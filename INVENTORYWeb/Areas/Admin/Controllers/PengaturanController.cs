using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace INVENTORYWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = OI.Role_SuperAdmin)]
    public class PengaturanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public PengaturanController(IUnitOfWork unitOfWork, 
            IWebHostEnvironment hostEnvironment, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {           

            PengaturanViewModel listData = new PengaturanViewModel()
            {
                ListData = _unitOfWork.ApplicationUser.GetAll().Where(z=>z.Flag == 0)
            };
            return View(listData);
        }
        public IActionResult Upsert(string id) 
        {           
            ViewBag.RoleList = _roleManager.Roles.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id
            });

            ApplicationUser applicationUser = new();

         
            if (string.IsNullOrEmpty(id))
            {
                return View(applicationUser);
            }
            else
            {
                applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == id);
                applicationUser.Role = _roleManager.Roles.Where(z => z.Name == applicationUser.RolesName).Select(i => i.Id).FirstOrDefault();

                return View(applicationUser);
            }
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ApplicationUser applicationUser)
        {
            var errorlist = ModelState.Values.SelectMany(x => x.Errors);
            if (ModelState.IsValid)
            {
                ApplicationUser oldUser = _unitOfWork.ApplicationUser.Get(applicationUser.Id);
                var newRoleId = _roleManager.Roles.Where(z => z.Id == applicationUser.Role).FirstOrDefault();
                await _userManager.RemoveFromRoleAsync(oldUser, oldUser.RolesName);
                await _userManager.AddToRoleAsync(oldUser, newRoleId.Name);
                oldUser.Role = applicationUser.Role;
                oldUser.RolesName = newRoleId.Name;
                _unitOfWork.ApplicationUser.Update(oldUser);
                _unitOfWork.Save();
                TempData["success"] = "User Berhasil Diupdate";
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }
    }
}
