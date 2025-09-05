using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace E_commerce_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unit;

        public CompanyController(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public IActionResult Index()
        {
            var companies = unit.Company.GetALL();
            return View(companies);
        }
        //API
        public IActionResult GetAll()
        {
            var companies = unit.Company.GetALL();
            return Json(new { data = companies });

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                unit.Company.Create(company);
                unit.save();
                TempData["success"] = "Company created syccesfully";
                return RedirectToAction("index");
            }

            return View(company);
        }

        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null) return NotFound();
            Company? company = unit.Company.GetByFilter(c => c.id == id);
            if (company == null) return NotFound();

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id,Company company)
        {
            if (id == 0 || id == null) return NotFound();
            if (ModelState.IsValid)
            {
                unit.Company.Update(company);
                unit.save();
                TempData["success"] = "Company updated syccesfully";
                return RedirectToAction("index");
            }

            return View(company);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null) return Json(new { success = false, Message = "Erore while deleting" });
            var company = unit.Company.GetByFilter(c=>c.id==id);
            if(company==null) return Json(new { success = false, Message = "Erore while deleting" });
            unit.Company.Delete(company);
            unit.save();
            return Json(new { success = true, Message = "Delete Successful" });
        }


    }


    
}
