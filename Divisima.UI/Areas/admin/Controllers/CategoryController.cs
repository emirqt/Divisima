using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.UI.Areas.admin.Controllers
{
	[Area("admin"), Authorize]
	public class CategoryController : Controller
	{
		IRepository<Category> repoCategory;
		public CategoryController(IRepository<Category> _repoCategory)
		{
			repoCategory = _repoCategory;
		}
		public IActionResult Index()
		{
			return View(repoCategory.GetAll().OrderBy(x => x.DisplayIndex));
		}

		public IActionResult New()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Category model)
		{
			if (ModelState.IsValid) // gelen değer doğrulanmışsa
			{
				repoCategory.Add(model);
				return RedirectToAction("Index");
			}
			else return RedirectToAction("New");

		}

		public IActionResult Edit(int id)
		{
			Category Category = repoCategory.GetBy(x => x.ID == id);
			if (Category != null) return View(Category);
			else return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category model)
		{
			if (ModelState.IsValid) // gelen değer doğrulanmışsa
			{
				repoCategory.Update(model);
				return RedirectToAction("Index");
			}
			else return RedirectToAction("New");

		}

		public IActionResult Delete(int id)
		{
			Category Category = repoCategory.GetBy(x => x.ID == id);
			if (Category != null)
			{
				repoCategory.Delete(Category);
			}
			return RedirectToAction("Index");
		}

	}
}
