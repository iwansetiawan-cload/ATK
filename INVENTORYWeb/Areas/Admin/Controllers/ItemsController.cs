using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace INVENTORYWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = OI.Role_SuperAdmin + "," + OI.Role_Admin)]
    public class ItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }     
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Items.GetAllAsync(includeProperties: "CATEGORY");
            return Json(new { data = allObj });
        }
        public async Task<IActionResult> Upsert(long? id)
        {

            ItemsViewModel itemVm = new()
            {
                Items = new(),
                SatuanList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.NAME,
                    Value = i.ID.ToString()
                }),
                IsiList = _unitOfWork.MSUDC.GetAll().Where(z=>z.ENTRY_KEY == "isi").Select(i => new SelectListItem { 
                    Text = i.INUM1.ToString(),
                    Value = i.INUM1.ToString()
                })
            };
            if (id == null || id == 0)
            {
                return View(itemVm);
            }
            else
            {
                itemVm.Items = await _unitOfWork.Items.GetFirstOrDefaultAsync(u => u.ID == id);
                return View(itemVm);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ItemsViewModel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Items.ID == 0)
                {
                    await _unitOfWork.Items.AddAsync(obj.Items);
                    TempData["Success"] = "Simpan Data Item";
                }
                else
                {
                    _unitOfWork.Items.Update(obj.Items);
                    TempData["UpdateSuccess"] = "Update Data Item";
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
