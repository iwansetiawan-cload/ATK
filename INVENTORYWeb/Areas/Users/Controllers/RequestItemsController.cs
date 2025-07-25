using Dapper;
using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Configuration;
using System.Data;
using System.Drawing.Drawing2D;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace INVENTORYWeb.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize(Roles = OI.Role_User + "," + OI.Role_SuperAdmin)]
    public class RequestItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static List<REQUEST_ITEM_DETAIL> AddListItems = new List<REQUEST_ITEM_DETAIL>();
        public RequestItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListView(string? status, string? date, int pg)
        {
            pg = pg == 0 ? 1 : pg;
            ViewBag.currentPage = pg;
            int pageSize = 12;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            RequestItemsListViewModel vm = new()
            {
                listData = _unitOfWork.RequestItemHeader.GetAll().Where(z => z.CREATE_BY == user.UserName && z.STATUS_ID >= 0).OrderBy(z => z.REQUEST_DATE),
            };

            if (!string.IsNullOrEmpty(status))
                vm.listData = vm.listData.Where(z=>z.STATUS == status).ToList();

            if(date == "month")
                vm.listData = vm.listData.Where(z => z.REQUEST_DATE.Value.Year == DateTime.Now.Year && z.REQUEST_DATE.Value.Month == DateTime.Now.Month).ToList();

            if (date == "year")
                vm.listData = vm.listData.Where(z => z.REQUEST_DATE.Value.Year == DateTime.Now.Year).ToList();

            if (date == "lastMonth")
                vm.listData = vm.listData.Where(z => z.REQUEST_DATE.Value.Year == DateTime.Now.AddMonths(-1).Year && z.REQUEST_DATE.Value.Month == DateTime.Now.AddMonths(-1).Month).ToList();

            if (date == "lastYear")
                vm.listData = vm.listData.Where(z => z.REQUEST_DATE.Value.Year == DateTime.Now.AddYears(-1).Year).ToList();

            int countNumber = vm.listData.Count();
            double totalPage = Math.Ceiling(countNumber / (double)pageSize);
            ViewBag.TotalPage = (int)totalPage;
            var skip = (pg - 1) * pageSize;

            vm.listData = vm.listData.Skip(skip).Take(pageSize).ToList();
            return View(vm);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            var allObj = (from z in _unitOfWork.RequestItemHeader.GetAll()
                          select new
                          {
                              id = z.ID,
                              project_name = z.PROJECT_NAME,
                              transaction_number = z.REQUEST_ORDER_NO,
                              request_date = z.REQUEST_DATE.HasValue ? z.REQUEST_DATE.Value.ToString("yyyy-MM-dd") : "",
                              notes = z.NOTES,
                              status_id = z.STATUS_ID,
                              status = z.STATUS,
                              row_number = z.ROW_NUMBER,
                              entry_by = z.CREATE_BY,
                          }).Where(x => x.entry_by == user.UserName && x.status_id >= 0);

            return Json(new { data = allObj });
        }

        [HttpGet]
        public IActionResult GetAllMasterProject()
        {
            MasterProjectViewModel obj = new MasterProjectViewModel();
            obj.ListData = _unitOfWork.SP_Call.List<MasterProject>(OI.Proc_Get_MasterProject);

            var allObj = (from z in _unitOfWork.MasterProject.GetAll()
                          select new
                          {
                              id = z.ID,
                              project_name = z.PROJECT_NAME
                          });
            return Json(new { data = allObj });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMasterItems()
        {
            var allObj = await _unitOfWork.Items.GetAllAsync(includeProperties: "CATEGORY");          
            return Json(new { data = allObj });
        }
        [HttpGet]
        public ActionResult ValidationCreateNew()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

                REQUEST_ITEM_HEADER rEQUEST_ITEM_HEADER = _unitOfWork.RequestItemHeader.GetAll().Where(z => z.CREATE_BY == user.UserName && z.STATUS_ID >= 0 && z.REQUEST_DATE.Value.Year == DateTime.Now.Year && z.REQUEST_DATE.Value.Month == DateTime.Now.Month).FirstOrDefault();
                if (rEQUEST_ITEM_HEADER != null)
                    return Json(new { success = true, message = "Anda sudah melakukan permintaan barang di bulan ini..!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = string.Empty });                
            }            

            return Json(new { success = false, message = string.Empty });
        }
        public IActionResult Upsert(long? id)
        {
            var listProject = _unitOfWork.MasterProject.GetAll().Select(x => new SelectListItem { Value = x.PROJECT_NAME, Text = x.PROJECT_NAME });
            ViewBag.ProjectList = new SelectList(listProject, "Value", "Text");
            AddListItems = new List<REQUEST_ITEM_DETAIL>();
            RequestItemHeaderViewModel vm = new()
            {
                REQUEST_ITEM_HEADER = new() {
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
                vm.REQUEST_ITEM_HEADER =  _unitOfWork.RequestItemHeader.Get(id);
                vm.ListItems = _unitOfWork.RequestItemDetail.GetAll(includeProperties : "ITEMS").Where(z=>z.HEADER_ID == id).ToList();

                foreach (var item in vm.ListItems)
                {                   
                    AddListItems.Add(item);
                }

                return View(vm);
            }

        }     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(RequestItemHeaderViewModel obj)
        {
            try
            {
                var listProject = _unitOfWork.MasterProject.GetAll().Select(x => new SelectListItem { Value = x.PROJECT_NAME, Text = x.PROJECT_NAME });
                ViewBag.ProjectList = new SelectList(listProject, "Value", "Text");

                if (obj.REQUEST_ITEM_HEADER.PROJECT_NAME == null)
                {
                    TempData["error"] = "Nama Project Harus diisi!";
                    return View(obj);
                }

                if (AddListItems.ToList().Count == 0)
                {
                    TempData["error"] = "Item Harus diisi!";
                    return View(obj);
                }                    

                var error = ModelState.Values.SelectMany(z => z.Errors);
                if (ModelState.IsValid)
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                    var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 0).FirstOrDefault();
                    int rowItem = 1;

                    if (obj.REQUEST_ITEM_HEADER.ID == 0)
                    {
                        long RowNUmber = 1;
                        List <REQUEST_ITEM_HEADER> ListHeader = _unitOfWork.RequestItemHeader.GetAll().ToList();
                        if (ListHeader.Count > 0)
                            RowNUmber = ListHeader.Max(z => z.ROW_NUMBER) + 1;
                        string RomawiString = IntToRoman(DateTime.Now.Month);
                        string RONumber = string.Join(string.Empty, "PR" + "/" + (RowNUmber.ToString().PadLeft(5, '0') + "/" + RomawiString + "/" + DateTime.Now.ToString("yy") + "/RDS"));

                        obj.REQUEST_ITEM_HEADER.TOTAL_QTY = AddListItems.Sum(z => z.QTY);
                        obj.REQUEST_ITEM_HEADER.REQUEST_ORDER_NO = RONumber;
                        obj.REQUEST_ITEM_HEADER.ROW_NUMBER = RowNUmber;
                        obj.REQUEST_ITEM_HEADER.CREATE_BY = user.UserName;
                        obj.REQUEST_ITEM_HEADER.CREATE_DATE = DateTime.Now;
                        obj.REQUEST_ITEM_HEADER.STATUS_ID = status?.INUM1;
                        obj.REQUEST_ITEM_HEADER.STATUS = status?.TEXT1;

                        _unitOfWork.RequestItemHeader.Add(obj.REQUEST_ITEM_HEADER);                        
                        foreach (var AddItems in AddListItems)
                        {
                            ITEMS iTEMS = await _unitOfWork.Items.GetAsync(AddItems.ITEMS.ID);
                            REQUEST_ITEM_DETAIL detail = new REQUEST_ITEM_DETAIL()
                            {
                                ITEM_NAME = AddItems.ITEM_NAME,
                                CATEGORY = AddItems.CATEGORY,
                                PIECE = AddItems.PIECE,
                                STOCK = AddItems.STOCK,
                                QTY = AddItems.QTY,
                                ITEMS = iTEMS,
                                REQUEST_ITEM_HEADER = obj.REQUEST_ITEM_HEADER,
                                STATUS = status?.TEXT1 ?? string.Empty,
                                STATUS_ID = status?.INUM1 ?? 0,
                                ROW_NUMBER = rowItem
                            };

                            _unitOfWork.RequestItemDetail.Add(detail);
                            _unitOfWork.Save();
                            rowItem++;

                            AuditTrailInfo auditTrailInfoDetail = new()
                            {
                                UserName = user.Name,
                                ModuleName = "RequestItems/Upsert",
                                TransactionId = detail.ID,
                                ActionName = status?.TEXT1 ?? string.Empty,
                                OtherInfo = string.Empty,
                                AuditTrailType = status?.INUM1 ?? 0,
                                ApplicationId = user.Id,
                                AuditTrailId = Guid.Empty
                            };
                            SaveAuditTrail(auditTrailInfoDetail);
                        }
                        AuditTrailInfo auditTrailInfo = new()
                        {
                            UserName = user.Name,
                            ModuleName = "RequestItems/Upsert",
                            TransactionId = obj.REQUEST_ITEM_HEADER.ID,
                            ActionName = status?.TEXT1 ?? string.Empty,
                            OtherInfo = string.Empty,
                            AuditTrailType = status?.INUM1 ?? 0,
                            ApplicationId = user.Id,
                            AuditTrailId = Guid.Empty
                        };
                        SaveAuditTrail(auditTrailInfo);
                        TempData["Success"] = "Simpan Permintaan Barang";
                    }
                    else
                    {
                        obj.REQUEST_ITEM_HEADER.TOTAL_QTY = AddListItems.Sum(z => z.QTY);
                        _unitOfWork.RequestItemHeader.Update(obj.REQUEST_ITEM_HEADER);

                        IEnumerable<REQUEST_ITEM_DETAIL> detailItemList = _unitOfWork.RequestItemDetail.GetAll().Where(z => z.HEADER_ID == obj.REQUEST_ITEM_HEADER.ID);
                        _unitOfWork.RequestItemDetail.RemoveRange(detailItemList);

                        REQUEST_ITEM_HEADER rEQUEST_ITEM_HEADER = _unitOfWork.RequestItemHeader.Get(obj.REQUEST_ITEM_HEADER.ID);
                        foreach (var AddItems in AddListItems)
                        {
                            ITEMS iTEMS = await _unitOfWork.Items.GetAsync(AddItems.ITEMS.ID);
                            REQUEST_ITEM_DETAIL detail = new REQUEST_ITEM_DETAIL()
                            {
                                ITEM_NAME = AddItems.ITEM_NAME,
                                CATEGORY = AddItems.CATEGORY,
                                PIECE = AddItems.PIECE,
                                STOCK = AddItems.STOCK,
                                QTY = AddItems.QTY,
                                ITEMS = iTEMS,
                                REQUEST_ITEM_HEADER = rEQUEST_ITEM_HEADER,
                                STATUS = status?.TEXT1 ?? string.Empty,
                                STATUS_ID = status?.INUM1 ?? 0,
                                ROW_NUMBER = rowItem
                            };

                            _unitOfWork.RequestItemDetail.Add(detail);
                            _unitOfWork.Save();
                            rowItem++;

                            AuditTrailInfo auditTrailInfoDetail = new()
                            {
                                UserName = user.Name,
                                ModuleName = "RequestItems/Upsert",
                                TransactionId = detail.ID,
                                ActionName = status?.TEXT1 ?? string.Empty,
                                OtherInfo = string.Empty,
                                AuditTrailType = status?.INUM1 ?? 0,
                                ApplicationId = user.Id,
                                AuditTrailId = Guid.Empty
                            };
                            SaveAuditTrail(auditTrailInfoDetail);
                        }                       
                        TempData["Success"] = "Update Permintaan Barang";

                    }
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Upsert), new { id = obj.REQUEST_ITEM_HEADER.ID });
                }
               
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            
            return View(obj);
        }

        [HttpPost]        
        public async Task<IActionResult> Create(string? projectname, string? notes, DateTime? requestdate, int? totalqty)
        {
            try
            {
                if (projectname == null)
                {
                    return Json(new { success = false, message = "Nama Project Harus diisi..!" });
                }
                if (AddListItems.ToList().Count == 0)
                {
                    return Json(new { success = false, message = "Item Harus diisi..!" });
                }

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 0).FirstOrDefault();

                long RowNUmber = 1;
                int rowItem = 1;
                List<REQUEST_ITEM_HEADER> ListHeader = _unitOfWork.RequestItemHeader.GetAll().ToList();
                if (ListHeader.Count > 0)
                    RowNUmber = ListHeader.Max(z => z.ROW_NUMBER) + 1;
                string RomawiString = IntToRoman(DateTime.Now.Month);
                string RONumber = string.Join(string.Empty, "PR" + "/" + (RowNUmber.ToString().PadLeft(5, '0') + "/" + RomawiString + "/" + DateTime.Now.ToString("yy") + "/RDS"));

                REQUEST_ITEM_HEADER rEQUEST_ITEM_HEADER = new()
                {
                    PROJECT_NAME = projectname,
                    NOTES = notes,
                    REQUEST_DATE = requestdate,
                    TOTAL_QTY = totalqty,
                    CREATE_BY = user.UserName,
                    CREATE_DATE = DateTime.Now,
                    STATUS_ID = status?.INUM1,
                    STATUS = status?.TEXT1,
                    REQUEST_ORDER_NO = RONumber,
                    ROW_NUMBER = RowNUmber,
                };
                _unitOfWork.RequestItemHeader.Add(rEQUEST_ITEM_HEADER);                
                foreach (var AddItems in AddListItems)
                {
                    ITEMS iTEMS = await _unitOfWork.Items.GetAsync(AddItems.ITEMS.ID);
                    REQUEST_ITEM_DETAIL detail = new REQUEST_ITEM_DETAIL()
                    {
                        ITEM_NAME = AddItems.ITEM_NAME,
                        CATEGORY = AddItems.CATEGORY,
                        PIECE = AddItems.PIECE,
                        STOCK = AddItems.STOCK,
                        QTY = AddItems.QTY,
                        ITEMS = iTEMS,
                        REQUEST_ITEM_HEADER = rEQUEST_ITEM_HEADER,
                        STATUS = status?.TEXT1 ?? string.Empty,
                        STATUS_ID = status?.INUM1 ?? 0,
                        ROW_NUMBER = rowItem
                    };

                    _unitOfWork.RequestItemDetail.Add(detail);
                    _unitOfWork.Save();
                    rowItem++;

                    AuditTrailInfo auditTrailInfoDetail = new()
                    {
                        UserName = user.Name,
                        ModuleName = "RequestItems/Upsert",
                        TransactionId = detail.ID,
                        ActionName = status?.TEXT1 ?? string.Empty,
                        OtherInfo = string.Empty,
                        AuditTrailType = status?.INUM1 ?? 0,
                        ApplicationId = user.Id,
                        AuditTrailId = Guid.Empty
                    };
                    SaveAuditTrail(auditTrailInfoDetail);
                }
                AuditTrailInfo auditTrailInfo = new()
                {
                    UserName = user.Name,
                    ModuleName = "RequestItems/Upsert",
                    TransactionId = rEQUEST_ITEM_HEADER.ID,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = string.Empty,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty
                };
                SaveAuditTrail(auditTrailInfo);
                _unitOfWork.Save();
                TempData["Success"] = "Simpan Permintaan Barang";
                return Json(new { success = true, message = "Simpan Berhasil", id = rEQUEST_ITEM_HEADER.ID });
                //return RedirectToAction(nameof(Upsert), new { id = rEQUEST_ITEM_HEADER.ID });
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TempData["Failed"] = "Simpan Gagal";
                return Json(new { success = false, message = "Simpan Gagal" });
            }
          
           
        }
        [HttpDelete]
        public IActionResult Delete(long id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == -1).FirstOrDefault();

            var objFromDb = _unitOfWork.RequestItemHeader.Get(id);
            objFromDb.STATUS_ID = status?.INUM1;
            objFromDb.STATUS = status?.TEXT1;
           
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Hapus Gagal" });
            }
            _unitOfWork.RequestItemHeader.Update(objFromDb);
            _unitOfWork.Save();

            AuditTrailInfo auditTrailInfo = new()
            {
                UserName = user.Name,
                ModuleName = "RequestItems/Delete",
                TransactionId = id,
                ActionName = status?.TEXT1 ?? string.Empty,
                OtherInfo = string.Empty,
                AuditTrailType = status?.INUM1 ?? 0,
                ApplicationId = user.Id,
                AuditTrailId = Guid.Empty
            };
            SaveAuditTrail(auditTrailInfo);
            TempData["Success"] = "Hapus Berhasil";
            return Json(new { success = true, message = "Hapus Berhasil" });
        }       
        public IActionResult ViewApproval(long? id, bool view)
        {
            ViewBag.View = view;
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
            int? totalQty = 0;   
            int? totalStock = 0;
            double? sumQty = 0;
            try
            {
                if (id == null)
                    return View(null);

                totalQty = qty ?? 0;
                totalStock = stock ?? 0;
                ITEMS items = await _unitOfWork.Items.GetFirstOrDefaultAsync(x=>x.ID == id, includeProperties: "CATEGORY");

                REQUEST_ITEM_DETAIL? dellItems = AddListItems.Where(z => z.ITEMS.ID == id).FirstOrDefault();
                if (dellItems != null)
                {
                    totalQty = (dellItems.QTY ?? 0) + totalQty;
                    totalStock = (dellItems.STOCK ?? 0) + totalStock ;
                    AddListItems.Remove(dellItems);
                }               
                sumQty = AddListItems.Sum(z => z.QTY) + totalQty;

                REQUEST_ITEM_DETAIL newDetail = new()
                {
                    ITEMS = items,
                    ITEM_ID = items.ID,
                    ITEM_NAME = items.NAME,
                    CATEGORY = items.CATEGORY.NAME,
                    PIECE = items.PIECE,
                    STOCK = totalStock,
                    QTY = totalQty
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
                total_qty = sumQty
            };
            return Json(res);
        }
        [HttpGet]
        public IActionResult GetAllItems()
        {
            var datalist = (from z in AddListItems.ToList()
                            select new
                            {
                                id = z.ITEMS.ID,
                                name = z.ITEMS.NAME,
                                satuan = z.CATEGORY,
                                qty = z.QTY,
                                stock = z.STOCK,
                                piece = z.PIECE,
                                norow = ""
                            }).OrderBy(x=>x.name).ToList();
            return Json(new { data = datalist });

        }
        [HttpGet]
        public IActionResult GetAllItemsView(int id)
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
        [HttpDelete]
        public IActionResult DeleteItem(long? id)
        {

            double? sumQty = 0;
            try
            {
                for (int i = 0; i < AddListItems.Count; i++)
                {
                    if (AddListItems[i].ITEMS.ID == id)
                    {
                        AddListItems.RemoveAt(i);
                    }
                }
                sumQty = AddListItems.Select(x => x.QTY).Sum();             

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            var res = new
            {
                result = "true",
                message = "Hapus Berhasil",
                total_qty = sumQty
            };
            return Json(res);
        }

        [HttpPost]
        public IActionResult UpdateCompleted(long id, string? notes)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                var status = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status" && z.INUM1 == 3).FirstOrDefault();

                REQUEST_ITEM_HEADER requestItemHeader = _unitOfWork.RequestItemHeader.Get(id);                

                if (requestItemHeader.STATUS_ID == status?.INUM1)
                {
                    TempData["Information"] = "Status sudah Complete..!";
                    return Json(new { success = false, message = "Status sudah Complete..!" });
                }
                requestItemHeader.STATUS_ID = status?.INUM1;
                requestItemHeader.STATUS = status?.TEXT1;
                requestItemHeader.COMPLETED_BY = user.UserName;
                requestItemHeader.COMPLETED_DATE = DateTime.Now;
                requestItemHeader.COMPLETED_NOTES = notes ?? "";
                _unitOfWork.RequestItemHeader.Update(requestItemHeader);

                List<REQUEST_ITEM_DETAIL> requestItemDetailList = _unitOfWork.RequestItemDetail.GetAll().Where(z => z.HEADER_ID == id && z.STATUS == "Process").ToList();
                foreach (var requestItemDetail in requestItemDetailList)
                {
                    requestItemDetail.STATUS_ID = status?.INUM1;
                    requestItemDetail.STATUS = status?.TEXT1;
                    requestItemDetail.COMPLETED_BY = user.UserName;
                    requestItemDetail.COMPLETED_DATE = DateTime.Now;
                    requestItemDetail.COMPLETED_NOTES = notes ?? "";
                    _unitOfWork.RequestItemDetail.Update(requestItemDetail);

                    AuditTrailInfo auditTrailInfoDetail = new()
                    {
                        UserName = user.Name,
                        ModuleName = "RequestItems/UpdateCompleted",
                        TransactionId = requestItemDetail.ID,
                        ActionName = status?.TEXT1 ?? string.Empty,
                        OtherInfo = notes ?? string.Empty,
                        AuditTrailType = status?.INUM1 ?? 0,
                        ApplicationId = user.Id,
                        AuditTrailId = Guid.Empty
                    };
                    SaveAuditTrail(auditTrailInfoDetail);
                }
                _unitOfWork.Save();

                AuditTrailInfo auditTrailInfo = new()
                {
                    UserName = user.Name,
                    ModuleName = "RequestItems/UpdateCompleted",
                    TransactionId = id,
                    ActionName = status?.TEXT1 ?? string.Empty,
                    OtherInfo = notes ?? string.Empty,
                    AuditTrailType = status?.INUM1 ?? 0,
                    ApplicationId = user.Id,
                    AuditTrailId = Guid.Empty
                };
                SaveAuditTrail(auditTrailInfo);
                TempData["Success"] = "Successfully Completed";
                return Json(new { success = true, message = "Process Completed" });
            }
            catch (Exception)
            {
                TempData["Failed"] = "Error Completed";
                return Json(new { success = false, message = "Process Error" });
            }

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
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }        
        private string IntToRoman(int num)
        {
            var result = string.Empty;
            var map = new Dictionary<string, int>
            {
                {"M", 1000 },
                {"CM", 900},
                {"D", 500},
                {"CD", 400},
                {"C", 100},
                {"XC", 90},
                {"L", 50},
                {"XL", 40},
                {"X", 10},
                {"IX", 9},
                {"V", 5},
                {"IV", 4},
                {"I", 1}
            };
            foreach (var pair in map)
            {
                result += string.Join(string.Empty, Enumerable.Repeat(pair.Key, num / pair.Value));
                num %= pair.Value;
            }
            return result;
        }
    }
}



