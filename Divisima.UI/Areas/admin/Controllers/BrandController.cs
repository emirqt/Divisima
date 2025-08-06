using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.UI.Areas.admin.Controllers
{
    [Area("admin"), Authorize]
    public class BrandController : Controller
    {
        IRepository<Brand> repoBrand;
        public BrandController(IRepository<Brand> _repoBrand)
        {
            repoBrand = _repoBrand;
        }
        public IActionResult Index()
        {
            return View(repoBrand.GetAll().OrderBy(x => x.DisplayIndex));
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Brand model)
        {
            if (ModelState.IsValid) // gelen değer doğrulanmışsa
            {
                repoBrand.Add(model);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("New");

        }

        public IActionResult Edit(int id)
        {
            Brand Brand = repoBrand.GetBy(x => x.ID == id);
            if (Brand != null) return View(Brand);
            else return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand model)
        {
            if (ModelState.IsValid) // gelen değer doğrulanmışsa
            {
                repoBrand.Update(model);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("New");

        }

        public IActionResult Delete(int id)
        {
            Brand Brand = repoBrand.GetBy(x => x.ID == id);
            if (Brand != null)
            {
                repoBrand.Delete(Brand);
            }
            return RedirectToAction("Index");
        }

    }
}
