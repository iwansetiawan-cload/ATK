using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
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
