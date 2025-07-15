using Dapper;
using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;
using System.Drawing.Drawing2D;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Data;
using Mono.TextTemplating;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static Azure.Core.HttpHeader;
using Microsoft.IdentityModel.Tokens;

namespace INVENTORYWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = OI.Role_SuperAdmin + "," + OI.Role_Admin)]
    public class RequestApprovalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static List<REQUEST_ITEM_DETAIL> AddListItems = new List<REQUEST_ITEM_DETAIL>();
        public RequestApprovalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var allObj = (from z in _unitOfWork.RequestItemHeader.GetAll() select new {
                id = z.ID,
                transaction_number = z.REQUEST_ORDER_NO,
                project_name = z.PROJECT_NAME,
                request_date = z.REQUEST_DATE.HasValue ? z.REQUEST_DATE.Value.ToString("yyyy-MM-dd") : "",
                notes = z.NOTES,
                status = z.STATUS,
                status_id = z.STATUS_ID
            }).Where(x=>x.status_id >= 0);
            if (status == "waitingApproval")
            {
                allObj = allObj.Where(z => z.status_id == 0).ToList();
            }
            else if (status == "approval")
            {
                allObj = allObj.Where(z => z.status_id == 1 || z.status_id == 2).ToList();
            }
            else if (status == "completed")
            {
                allObj = allObj.Where(z => z.status_id == 3).ToList();
            }

            return Json(new { data = allObj });
        }
       
        [HttpGet]
        public IActionResult GetAllItems(int id)
        {
            var datalist = (from z in _unitOfWork.RequestItemDetail.GetAll(includeProperties: "ITEMS").Where(i => i.HEADER_ID == id).ToList()
            select new
                            {
                                id = z.ID,
                                name = z.ITEMS.NAME,
                                satuan = z.CATEGORY,
                                qty = z.QTY,
                                piece = z.PIECE,
                                status = z.STATUS,
                                notes = z.REJECTED_NOTES,
                                adjust = z.QTY_ADJUST,
                            }).ToList();
            return Json(new { data = datalist });

        }
        [HttpPost]
        public IActionResult ApproveHeader(long id)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 1).FirstOrDefault();

                REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(id);
                requestItemHeader.STATUS_ID = status?.INUM1;
                requestItemHeader.STATUS = status?.TEXT1;
                requestItemHeader.APPROVE_BY = user.UserName;
                requestItemHeader.APPROVE_DATE = DateTime.Now;
                requestItemHeader.REJECTED_BY = null;
                requestItemHeader.REJECTED_DATE = null;
                requestItemHeader.REJECTED_NOTES = null;
                _unitOfWork.RequestItemHeader.Update(requestItemHeader);

                List<REQUEST_ITEM_DETAIL> requestItemDetailList = _unitOfWork.RequestItemDetail.GetAll().Where(z=>z.HEADER_ID == id).ToList();
                foreach (var requestItemDetail in requestItemDetailList)
                {                    
                    requestItemDetail.STATUS_ID = status?.INUM1;
                    requestItemDetail.STATUS = status?.TEXT1;
                    requestItemDetail.APPROVE_BY = user.UserName;
                    requestItemDetail.APPROVE_DATE = DateTime.Now;
                    requestItemDetail.REJECTED_BY = null;
                    requestItemDetail.REJECTED_DATE = null;
                    requestItemDetail.REJECTED_NOTES = null;
                    requestItemDetail.QTY_ADJUST = null;
                    _unitOfWork.RequestItemDetail.Update(requestItemDetail);

                    AuditTrailInfo auditTrailInfoDetail = new()
                    {
                        UserName = user.Name,
                        ModuleName = "RequestApproval/ApproveHeader",
                        TransactionId = requestItemDetail.ID,
                        ActionName = status?.TEXT1 ?? string.Empty,
                        OtherInfo = string.Empty,
                        AuditTrailType = status?.INUM1 ?? 0,
                        ApplicationId = user.Id,
                        AuditTrailId = Guid.Empty,
                    };
                    SaveAuditTrail(auditTrailInfoDetail);
                }
                _unitOfWork.Save();

                AuditTrailInfo auditTrailInfo = new(){
                    UserName = user.Name,
                    ModuleName = "RequestApproval/ApproveHeader",
                    TransactionId = id,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = string.Empty,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty
                };
                SaveAuditTrail(auditTrailInfo);
                TempData["Success"] = "Successfully approved";
                return Json(new { success = true, message = "Approved Successful" , status = status?.TEXT1 ?? string.Empty });
            }
            catch (Exception)
            {
                TempData["Failed"] = "Error approved";
                return Json(new { success = false, message = "Approved Error"});
            }

        }
        [HttpDelete]
        public IActionResult RejectHeader(long id, string? notes)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 2).FirstOrDefault();

                REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(id);
                requestItemHeader.STATUS_ID = status?.INUM1;
                requestItemHeader.STATUS = status?.TEXT1;
                requestItemHeader.REJECTED_BY = user.UserName;
                requestItemHeader.REJECTED_DATE = DateTime.Now;
                requestItemHeader.REJECTED_NOTES = notes;
                requestItemHeader.APPROVE_BY = null;
                requestItemHeader.APPROVE_DATE = null;
                _unitOfWork.RequestItemHeader.Update(requestItemHeader);               

                List<REQUEST_ITEM_DETAIL> requestItemDetailList = _unitOfWork.RequestItemDetail.GetAll().Where(z => z.HEADER_ID == id).ToList();
                foreach (var requestItemDetail in requestItemDetailList)
                {
                    requestItemDetail.STATUS_ID = status?.INUM1;
                    requestItemDetail.STATUS = status?.TEXT1;
                    requestItemDetail.REJECTED_BY = user.UserName;
                    requestItemDetail.REJECTED_DATE = DateTime.Now;
                    requestItemDetail.REJECTED_NOTES = notes;
                    requestItemDetail.APPROVE_BY = null;
                    requestItemDetail.APPROVE_DATE = null;
                    requestItemDetail.QTY_ADJUST = null;
                    _unitOfWork.RequestItemDetail.Update(requestItemDetail);

                    AuditTrailInfo auditTrailInfoDetail = new()
                    {
                        UserName = user.Name,
                        ModuleName = "RequestApproval/RejectHeader",
                        TransactionId = requestItemDetail.ID,
                        ActionName = status?.TEXT1 ?? string.Empty,
                        OtherInfo = notes ?? string.Empty,
                        AuditTrailType = status?.INUM1 ?? 0,
                        ApplicationId = user.Id,
                        AuditTrailId = Guid.Empty,
                    };
                    SaveAuditTrail(auditTrailInfoDetail);
                }
                _unitOfWork.Save();

                AuditTrailInfo auditTrailInfo = new()
                {
                    UserName = user.Name,
                    ModuleName = "RequestApproval/RejectHeader",
                    TransactionId = id,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = notes ?? string.Empty,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty
                };
                SaveAuditTrail(auditTrailInfo);

                TempData["Success"] = "Successfully Reject";
                return Json(new { success = true, message = "Reject Successful", status = status?.TEXT1 ?? string.Empty });
            }
            catch (Exception)
            {
                TempData["Failed"] = "Error Reject";
                return Json(new { success = false, message = "Reject Error" });
            }

        }
        [HttpPost]
        public IActionResult ApproveDetail(long id)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 1).FirstOrDefault();

                REQUEST_ITEM_DETAIL requestItemDetail = _unitOfWork.RequestItemDetail.Get(id);
                requestItemDetail.STATUS_ID = status?.INUM1;
                requestItemDetail.STATUS = status?.TEXT1;
                requestItemDetail.APPROVE_BY = user.UserName;
                requestItemDetail.APPROVE_DATE = DateTime.Now;
                requestItemDetail.REJECTED_BY = null;
                requestItemDetail.REJECTED_DATE = null;
                requestItemDetail.REJECTED_NOTES = null;
                requestItemDetail.QTY_ADJUST = null;
                _unitOfWork.RequestItemDetail.Update(requestItemDetail);

                #region Update Header
                REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(requestItemDetail.HEADER_ID);
                requestItemHeader.STATUS_ID = status?.INUM1;
                requestItemHeader.STATUS = status?.TEXT1;
                requestItemHeader.APPROVE_BY = user.UserName;
                requestItemHeader.APPROVE_DATE = DateTime.Now;
                requestItemHeader.REJECTED_BY = null;
                requestItemHeader.REJECTED_DATE = null;
                requestItemHeader.REJECTED_NOTES = null;
                _unitOfWork.RequestItemHeader.Update(requestItemHeader);
                #endregion

                _unitOfWork.Save();
                AuditTrailInfo auditTrailInfo = new()
                {
                    UserName = user.Name,
                    ModuleName = "RequestApproval/ApproveDetail",
                    TransactionId = id,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = string.Empty,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty
                };
                SaveAuditTrail(auditTrailInfo);
               
                TempData["Success"] = "Successfully approved";
                return Json(new { success = true, message = "Approved Successful", status = status?.TEXT1 ?? string.Empty });
            }
            catch (Exception)
            {
                TempData["Failed"] = "Error approved";
                return Json(new { success = false, message = "Approved Error" });
            }

        }
        [HttpDelete]
        public IActionResult RejectDetail(long id, string? notes)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 2).FirstOrDefault();

                REQUEST_ITEM_DETAIL requestItemDetail = _unitOfWork.RequestItemDetail.Get(id);
                requestItemDetail.STATUS_ID = status?.INUM1;
                requestItemDetail.STATUS = status?.TEXT1;
                requestItemDetail.REJECTED_BY = user.UserName;
                requestItemDetail.REJECTED_DATE = DateTime.Now;
                requestItemDetail.REJECTED_NOTES = notes;
                requestItemDetail.APPROVE_BY = null;
                requestItemDetail.APPROVE_DATE = null;
                requestItemDetail.QTY_ADJUST = null;
                _unitOfWork.RequestItemDetail.Update(requestItemDetail);

                List<REQUEST_ITEM_DETAIL> requestItemDetailList = _unitOfWork.RequestItemDetail.GetAll().Where(z => z.HEADER_ID == requestItemDetail.HEADER_ID && z.STATUS_ID == 1).ToList();
                if (requestItemDetailList.Count == 0)
                {
                    REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(requestItemDetail.HEADER_ID);
                    requestItemHeader.STATUS_ID = status?.INUM1;
                    requestItemHeader.STATUS = status?.TEXT1;
                    requestItemHeader.REJECTED_BY = user.UserName;
                    requestItemHeader.REJECTED_DATE = DateTime.Now;
                    requestItemHeader.REJECTED_NOTES = notes;
                    requestItemHeader.APPROVE_BY = null;
                    requestItemHeader.APPROVE_DATE = null;
                    _unitOfWork.RequestItemHeader.Update(requestItemHeader);
                }
                _unitOfWork.Save();
                AuditTrailInfo auditTrailInfo = new()
                {
                    UserName = user.Name,
                    ModuleName = "RequestApproval/RejectDetail",
                    TransactionId = id,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = notes ?? string.Empty,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty,
                };
                SaveAuditTrail(auditTrailInfo);
                TempData["Success"] = "Successfully Reject";
                if (requestItemDetailList.Count == 0)
                    return Json(new { success = true, message = "Reject Successful", status = status?.TEXT1 ?? string.Empty });
                else
                    return Json(new { success = true, message = "Reject Successful" , status  = ""});
            }
            catch (Exception)
            {
                TempData["Failed"] = "Error Reject";
                return Json(new { success = false, message = "Reject Error" });
            }

        }
        public IActionResult ProcessApproval(long? id)
        {
            AddListItems = new List<REQUEST_ITEM_DETAIL>();
            RequestItemHeaderViewModel vm = new()
            {
                REQUEST_ITEM_HEADER = new()
                {
                    REQUEST_DATE = DateTime.Now,
                },
                REQUEST_ITEM_DETAIL = new(),
            };
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.REQUEST_ITEM_HEADER = _unitOfWork.RequestItemHeader.Get(id);
                vm.ListItems = _unitOfWork.RequestItemDetail.GetAll(includeProperties: "ITEMS").Where(z => z.HEADER_ID == id).ToList();

                return View(vm);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddItem(long? id, int? qty, int? stock)
        {
            double? totalQty = 0;
            try
            {
                totalQty = AddListItems.Sum(z => z.QTY) + qty;
                ITEMS items = await _unitOfWork.Items.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "CATEGORY");

                REQUEST_ITEM_DETAIL newDetail = new()
                {
                    ITEMS = items,
                    ITEM_ID = items.ID,
                    ITEM_NAME = items.NAME,
                    CATEGORY = items.CATEGORY.NAME,
                    PIECE = items.PIECE,
                    STOCK = stock,
                    QTY = qty
                };

                AddListItems.Add(newDetail);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            var res = new
            {
                result = "true",
                message = "Tambah Berhasil",
                total_qty = totalQty
            };
            return Json(res);
        }
        private void SaveAuditTrail(AuditTrailInfo AuditTrailInfo)
        {
            try
            {                   
                var parameter = new DynamicParameters();
                parameter.Add("@UserName", AuditTrailInfo.UserName);
                parameter.Add("@ModuleName", AuditTrailInfo.ModuleName);
                parameter.Add("@TransactionId", AuditTrailInfo.TransactionId);
                parameter.Add("@ActionName", AuditTrailInfo.ActionName);
                parameter.Add("@OtherInfo", AuditTrailInfo.OtherInfo);
                parameter.Add("@AuditTrailType", AuditTrailInfo.AuditTrailType);
                parameter.Add("@ApplicationId", AuditTrailInfo.ApplicationId);
                parameter.Add("@AuditTrailId", AuditTrailInfo.AuditTrailId, direction: ParameterDirection.Output);
                _unitOfWork.SP_Call.Execute(OI.Proc_AuditTrail_SaveHeader, parameter);
                //Guid result = parameter.Get<Guid>("@AuditTrailId");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }            
        }
        [HttpGet]
        public IActionResult GetHistory(long id)
        {
            DataHistoryList obj = new DataHistoryList();

            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            obj.ListData = _unitOfWork.SP_Call.List<HistoryApproval>(OI.Proc_Get_AuditTrail_ById, parameter);

            var datalist = (from z in obj.ListData
                            select new
                            {
                                status = z.Status,
                                entryBy = z.EntryBy,
                                entryDate = z.EntryDate,
                                notes = z.Notes,
                            }).ToList();

            return Json(new { data = datalist });

        }
        [HttpPost]
        public IActionResult AddAdjustItem(long id, int? qty, string notes)
        {
            try
            {
                if (qty == null)
                {
                    TempData["Failed"] = "Error Simpan";
                    return Json(new { success = false, message = "Qty harus diisi" });
                }
                if (string.IsNullOrEmpty(notes))
                {
                    TempData["Failed"] = "Error Simpan";
                    return Json(new { success = false, message = "Keterangan harus diisi" });
                }

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 1).FirstOrDefault();

                REQUEST_ITEM_DETAIL requestItemDetail = _unitOfWork.RequestItemDetail.Get(id);
                requestItemDetail.STATUS_ID = status?.INUM1;
                requestItemDetail.STATUS = status?.TEXT1;
                requestItemDetail.APPROVE_BY = user.UserName;
                requestItemDetail.APPROVE_DATE = DateTime.Now;
                requestItemDetail.REJECTED_BY = null;
                requestItemDetail.REJECTED_DATE = null;
                requestItemDetail.REJECTED_NOTES = null;
                requestItemDetail.QTY_ADJUST = qty;
                requestItemDetail.REJECTED_NOTES = notes;
                _unitOfWork.RequestItemDetail.Update(requestItemDetail);

                #region Update Header
                REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(requestItemDetail.HEADER_ID);
                requestItemHeader.STATUS_ID = status?.INUM1;
                requestItemHeader.STATUS = status?.TEXT1;
                requestItemHeader.APPROVE_BY = user.UserName;
                requestItemHeader.APPROVE_DATE = DateTime.Now;
                requestItemHeader.REJECTED_BY = null;
                requestItemHeader.REJECTED_DATE = null;
                requestItemHeader.REJECTED_NOTES = null;
                _unitOfWork.RequestItemHeader.Update(requestItemHeader);
                #endregion

                _unitOfWork.Save();

                AuditTrailInfo auditTrailInfo = new()
                {
                    UserName = user.Name,
                    ModuleName = "RequestApproval/AddAdjustItem",
                    TransactionId = id,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = notes,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty
                };
                SaveAuditTrail(auditTrailInfo);

                TempData["Success"] = "Simpan Berhasil";        
                return Json(new { success = true, message = "Data Adjust disimpan", status = status?.TEXT1 ?? string.Empty });

            }
            catch (Exception)
            {
                TempData["Failed"] = "Error Simpan";
                return Json(new { success = false, message = "Data Adjust Error" });
            }

        }
        [HttpGet]
        public IActionResult ValApproveHeader(long id, string status)
        {
            REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(id);
            if (requestItemHeader != null)
            {
                if (requestItemHeader.STATUS == status)
                {
                    TempData["Failed"] = "Error validation";
                    return Json(new { success = true, message = "Status Ready " + status });
                }
            }
            TempData["Failed"] = "Error validation";
            return Json(new { success = false, message = "" });
        }
        [HttpGet]
        public IActionResult ValApproveDetail(long id, string status)
        {
            REQUEST_ITEM_DETAIL requestItemDetail = _unitOfWork.RequestItemDetail.Get(id);
            if (requestItemDetail != null) {
                if (requestItemDetail.STATUS == status)
                {
                    TempData["Failed"] = "Error validation";
                    return Json(new { success = true, message = "Status Ready " + status });
                }
            }
            TempData["Failed"] = "Error validation";
            return Json(new { success = false, message = "" });
        }
    }
}



