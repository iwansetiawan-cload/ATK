using INVENTORYWeb.DataAccess.Repository;
using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace INVENTORYWeb.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize(Roles = OI.Role_User + "," + OI.Role_SuperAdmin)]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;        

        public HomeController( IUnitOfWork unitOfWork)
        {
            //_logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            RequestItemsListViewModel vm = new()
            {
                listData = _unitOfWork.RequestItemHeader.GetAll().Where(z => z.CREATE_BY == user.UserName && z.STATUS_ID >= 0)
            };

            return View(vm);
        }
        public IActionResult RequestItemsList()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            RequestItemsListViewModel vm = new()
            {
                listData = _unitOfWork.RequestItemHeader.GetAll().Where(z=>z.CREATE_BY == user.UserName && z.STATUS_ID >= 0)
            };
           
            return View(vm);
        }
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
