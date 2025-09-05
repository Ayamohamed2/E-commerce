using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Utility;
using E_commerce_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unit;

        public CategoryController(IUnitOfWork _unit)
        {

            unit = _unit;
        }
        public IActionResult Index()
        {
            List<Category> categories = unit.Category.GetALL();
            return View(categories);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                unit.Category.Create(model);
                unit.save();
                TempData["success"] = "Category created syccesfully";
                return RedirectToAction("index");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Category? model = unit.Category.GetByFilter(c => c.Id == id);
            if (model == null) return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, Category data)
        {
            if (id == null || id == 0) return NotFound();
            if (ModelState.IsValid)
            {
                unit.Category.Update(data);
                unit.save();
                TempData["success"] = "Category updated syccesfully";
                return RedirectToAction("index");
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Category? model = unit.Category.GetByFilter(c => c.Id == id);
            if (model == null) return NotFound();

            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult Delete_(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Category? model = unit.Category.GetByFilter(c => c.Id == id);
            if (model == null) return NotFound();
            unit.Category.Delete(model);
            unit.save();
            TempData["success"] = "Category deleted syccesfully";
            return RedirectToAction("index");
        }
    }
}
