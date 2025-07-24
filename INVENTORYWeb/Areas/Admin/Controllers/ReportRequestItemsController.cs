using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace INVENTORYWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = OI.Role_SuperAdmin + "," + OI.Role_Admin)]
    public class ReportRequestItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportRequestItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(ReportRequestItemsViewModel vm)
        {           
            ViewBag.listStatus = _unitOfWork.MSUDC.GetAll().Where(z => z.ENTRY_KEY == "status").Select(x => new SelectListItem { Value = x.TEXT1, Text = x.TEXT1 });        
            vm.listData = _unitOfWork.RequestItemHeader.GetAll();
            if(!string.IsNullOrEmpty(vm.searchRequestNumber))
                vm.listData = vm.listData.Where(z=> vm.searchRequestNumber.Contains(z.REQUEST_ORDER_NO ?? string.Empty)).ToList();
            if (!string.IsNullOrEmpty(vm.searchProjectName))
                vm.listData = vm.listData.Where(z => vm.searchProjectName.Contains(z.PROJECT_NAME)).ToList();
            if (!string.IsNullOrEmpty(vm.searchStatus))
                vm.listData = vm.listData.Where(z => z.STATUS == vm.searchStatus).ToList();

            return View(vm);
        }
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var allObj = (from z in _unitOfWork.RequestItemHeader.GetAll()
                          select new
                          {
                              id = z.ID,
                              transaction_number = z.REQUEST_ORDER_NO,
                              project_name = z.PROJECT_NAME,
                              request_date = z.REQUEST_DATE.HasValue ? z.REQUEST_DATE.Value.ToString("yyyy-MM-dd") : "",
                              notes = z.NOTES,
                              status = z.STATUS,
                              status_id = z.STATUS_ID
                          });
          
            return Json(new { data = allObj });
        }
    }
}
