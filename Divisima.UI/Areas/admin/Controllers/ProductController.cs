using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Divisima.UI.Areas.admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.UI.Areas.admin.Controllers
{
    [Area("admin"), Authorize]
    public class ProductController : Controller
    {
        IRepository<Product> repoProduct;
        IRepository<Category> repoCategory;
        IRepository<Brand> repoBrand;
        
        public ProductController(IRepository<Product> _repoProduct, IRepository<Category> _repoCategory, IRepository<Brand> _repoBrand)
        {
            repoProduct = _repoProduct;
            repoCategory = _repoCategory;
            repoBrand = _repoBrand;
        }
        public IActionResult Index()
        {
            return View(repoProduct.GetAll().OrderBy(x => x.DisplayIndex));
        }

        public IActionResult New()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                Brands = repoBrand.GetAll().OrderBy(x => x.Name),
                Categories = repoCategory.GetAll().OrderBy(x => x.Name)
            };
            return View(productVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductVM model)
        {
            if (ModelState.IsValid) // gelen değer doğrulanmışsa
            {
                repoProduct.Add(model.Product);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("New");

        }

        public IActionResult Edit(int id)
        {
            Product product = repoProduct.GetBy(x => x.ID == id);
            ProductVM productVM = new ProductVM()
            {
                Product = product,
                Brands = repoBrand.GetAll().OrderBy(x => x.Name),
                Categories = repoCategory.GetAll().OrderBy(x => x.Name)
            };
            if (product != null) return View(productVM);
            else return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM model)
        {
            if (ModelState.IsValid) // gelen değer doğrulanmışsa
            {
                repoProduct.Update(model.Product);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("New");

        }

        public IActionResult Delete(int id)
        {
            Product Product = repoProduct.GetBy(x => x.ID == id);
            if (Product != null)
            {
                repoProduct.Delete(Product);
            }
            return RedirectToAction("Index");
        }

    }
}
