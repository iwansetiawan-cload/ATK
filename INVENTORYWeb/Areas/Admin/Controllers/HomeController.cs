using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace INVENTORYWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = OI.Role_SuperAdmin + "," + OI.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(DashboardViewModel vm)
        {
            List<REQUEST_ITEM_HEADER> RequestHeader = _unitOfWork.RequestItemHeader.GetAll().ToList();
            vm.MonthlyTransactionList = RequestHeader.Where(z=> z.REQUEST_DATE.Value.Month == DateTime.Now.Month && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).ToList() ?? null;
            vm.Status_NewRequest = RequestHeader.Where(z => z.STATUS == "New Request").Count();
            vm.Status_Process = RequestHeader.Where(z => z.STATUS == "Process").Count();
            //vm.Status_Cancel = RequestHeader.Where(z => z.STATUS == "Cancel").Count();
            vm.Status_ApproveAndReject = RequestHeader.Where(z => z.STATUS == "Approve" || z.STATUS == "Rejected").Count();
            vm.Status_Complete = RequestHeader.Where(z => z.STATUS == "Complete").Count();

            vm.Status_NewRequest_Current = RequestHeader.Where(z => z.STATUS == "New Request" && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).Count();
            vm.Status_Approve_Current = RequestHeader.Where(z => z.STATUS == "Approve" && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).Count();
            vm.Status_Reject_Current = RequestHeader.Where(z => z.STATUS == "Rejected" && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).Count();
            vm.Status_Process_Current = RequestHeader.Where(z => z.STATUS == "Process" && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).Count();          
            vm.Status_Complete_Current = RequestHeader.Where(z => z.STATUS == "Complete" && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).Count();

            int sumStatus = RequestHeader.Where(z => z.STATUS != "Cancel" && z.REQUEST_DATE.Value.Year == DateTime.Now.Year).Count();           
            var newRequest = (double)vm.Status_NewRequest_Current * 100 / (double)sumStatus;
            var approve = (double)vm.Status_Approve_Current * 100 / (double)sumStatus;
            var reject = (double)vm.Status_Reject_Current * 100 / (double)sumStatus;
            var process = (double)vm.Status_Process_Current * 100 / (double)sumStatus;
            var complete = (double)vm.Status_Complete_Current * 100 / (double)sumStatus;

            vm.TotalRequestTahunan = sumStatus;
            vm.Persen_NewRequest_Current = Math.Round(newRequest,2);
            vm.Persen_Approve_Current = Math.Round(approve,2);
            vm.Persen_Reject_Current = Math.Round(reject, 2);
            vm.Persen_Process_Current = Math.Round(process, 2);
            vm.Persen_Complete_Current = Math.Round(complete, 2);

            vm.Persen_NewRequest = (int)vm.Persen_NewRequest_Current;
            vm.Persen_Approve = (int)vm.Persen_Approve_Current;
            vm.Persen_Reject = (int)vm.Persen_Reject_Current;
            vm.Persen_Process = (int)vm.Persen_Process_Current;
            vm.Persen_Complete = (int)vm.Persen_Complete_Current;


            #region Pergerakan Barang Tahunan
            List<MSUDC> mSUDCList = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "daskboard_movement_items").ToList();
            vm.mSUDCList = mSUDCList;

            string? item1 = mSUDCList.Where(z => z.INUM1 == 1).Select(i => i.TEXT1).FirstOrDefault();
            string? item2 = mSUDCList.Where(z => z.INUM1 == 2).Select(i => i.TEXT1).FirstOrDefault();
            string? item3 = mSUDCList.Where(z => z.INUM1 == 3).Select(i => i.TEXT1).FirstOrDefault();
            string? item4 = mSUDCList.Where(z => z.INUM1 == 4).Select(i => i.TEXT1).FirstOrDefault();
            string? item5 = mSUDCList.Where(z => z.INUM1 == 5).Select(i => i.TEXT1).FirstOrDefault();

            vm.Item1 = item1;
            vm.Item2 = item2;
            vm.Item3 = item3;
            vm.Item4 = item4;
            vm.Item5 = item5;

            //vm.Jan_Item1 = _unitOfWork.RequestItemDetail.GetAll().Where(z => z.ITEM_NAME == item1 && z.da.Value.Year == DateTime.Now.Year).Count();
            //vm.Jan_Item2 = _unitOfWork.RequestItemDetail.GetAll().Where(z => z.ITEM_NAME == item2 && z.REJECTED_DATE.Value.Year == DateTime.Now.Year).Count();

            //int months = _unitOfWork.RequestItemDetail.GetFirstOrDefault().REJECTED_DATE.Value.Month;
           
            var getDataDashboard = _unitOfWork.SP_Call.List<Proc_GetDataDashboard>(OI.Proc_Dashboard_Get_Items);

            vm.Jan_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 1).Select(o => o.QTY).FirstOrDefault();
            vm.Jan_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 1).Select(o => o.QTY).FirstOrDefault();
            vm.Jan_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 1).Select(o => o.QTY).FirstOrDefault();
            vm.Jan_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 1).Select(o => o.QTY).FirstOrDefault();
            vm.Jan_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 1).Select(o => o.QTY).FirstOrDefault();

            vm.Feb_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 2).Select(o => o.QTY).FirstOrDefault();
            vm.Feb_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 2).Select(o => o.QTY).FirstOrDefault();
            vm.Feb_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 2).Select(o => o.QTY).FirstOrDefault();
            vm.Feb_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 2).Select(o => o.QTY).FirstOrDefault();
            vm.Feb_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 2).Select(o => o.QTY).FirstOrDefault();

            vm.Mar_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 3).Select(o => o.QTY).FirstOrDefault();
            vm.Mar_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 3).Select(o => o.QTY).FirstOrDefault();
            vm.Mar_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 3).Select(o => o.QTY).FirstOrDefault();
            vm.Mar_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 3).Select(o => o.QTY).FirstOrDefault();
            vm.Mar_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 3).Select(o => o.QTY).FirstOrDefault();

            vm.Apr_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 4).Select(o => o.QTY).FirstOrDefault();
            vm.Apr_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 4).Select(o => o.QTY).FirstOrDefault();
            vm.Apr_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 4).Select(o => o.QTY).FirstOrDefault();
            vm.Apr_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 4).Select(o => o.QTY).FirstOrDefault();
            vm.Apr_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 4).Select(o => o.QTY).FirstOrDefault();

            vm.May_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 5).Select(o => o.QTY).FirstOrDefault();
            vm.May_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 5).Select(o => o.QTY).FirstOrDefault();
            vm.May_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 5).Select(o => o.QTY).FirstOrDefault();
            vm.May_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 5).Select(o => o.QTY).FirstOrDefault();
            vm.May_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 5).Select(o => o.QTY).FirstOrDefault();

            vm.Jun_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 6).Select(o => o.QTY).FirstOrDefault();
            vm.Jun_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 6).Select(o => o.QTY).FirstOrDefault();
            vm.Jun_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 6).Select(o => o.QTY).FirstOrDefault();
            vm.Jun_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 6).Select(o => o.QTY).FirstOrDefault();
            vm.Jun_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 6).Select(o => o.QTY).FirstOrDefault();

            vm.Jul_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 7).Select(o => o.QTY).FirstOrDefault();
            vm.Jul_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 7).Select(o => o.QTY).FirstOrDefault();
            vm.Jul_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 7).Select(o => o.QTY).FirstOrDefault();
            vm.Jul_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 7).Select(o => o.QTY).FirstOrDefault();
            vm.Jul_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 7).Select(o => o.QTY).FirstOrDefault();

            vm.Aug_Item1 = getDataDashboard.Where(z => z.ITEM_NAME == item1 & z.FLAG == 8).Select(o => o.QTY).FirstOrDefault();
            vm.Aug_Item2 = getDataDashboard.Where(z => z.ITEM_NAME == item2 & z.FLAG == 8).Select(o => o.QTY).FirstOrDefault();
            vm.Aug_Item3 = getDataDashboard.Where(z => z.ITEM_NAME == item3 & z.FLAG == 8).Select(o => o.QTY).FirstOrDefault();
            vm.Aug_Item4 = getDataDashboard.Where(z => z.ITEM_NAME == item4 & z.FLAG == 8).Select(o => o.QTY).FirstOrDefault();
            vm.Aug_Item5 = getDataDashboard.Where(z => z.ITEM_NAME == item5 & z.FLAG == 8).Select(o => o.QTY).FirstOrDefault();


            //if (getDataDashboard != null)
            //{
            //    vm.GetDataDashboard = getDataDashboard.ToList();

            //    foreach (var obj in vm.GetDataDashboard)
            //    {
            //        if (obj.ITEM_NAME == item1)
            //        {

            //            if (obj.FLAG == 1)
            //            {
            //                vm.Jan_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 2) 
            //            {
            //                vm.Feb_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 3)
            //            {
            //                vm.Mar_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 4)
            //            {
            //                vm.Apr_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 5)
            //            {
            //                vm.May_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 6)
            //            {
            //                vm.Jun_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 7)
            //            {
            //                vm.Jul_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 8)
            //            {
            //                vm.Aug_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 9)
            //            {
            //                vm.Sep_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 10)
            //            {
            //                vm.Okt_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 11)
            //            {
            //                vm.Nov_Item1 = obj.QTY;
            //            }
            //            else if (obj.FLAG == 12)
            //            {
            //                vm.Des_Item1 = obj.QTY;
            //            }
            //        }

            //    }


            //}



            #endregion





            return View(vm);
        }
    }
}
