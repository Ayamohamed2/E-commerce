using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce.Model.ViewModels;
using E_commerce_project.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class FeedbackController : Controller
    {
        private readonly IUnitOfWork unit;

        public FeedbackController(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Feedback(int? id)
        {
            if (id == null || id == 0) return NotFound();
         
            var product = unit.Product.GetByFilter(p => p.Id == id, Includes: "ProductImages,Category");
            if (product == null) return NotFound();

            var feedbacks = unit.Feedback.GetALL(f => f.Product_Id == id).OrderByDescending(p => p.CreatedAt).ToList();

            var avgRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;

            foreach(var f in feedbacks)
            {
                var user = unit.User.GetByFilter(u => u.Id == f.User_Id);
                f.Profile_image = user.ProfilePicture;
            }

            var feedbackVM = new FeedbackVM
            {
                product = product,
                Feedbacks = feedbacks,
                AverageRating =(double) avgRating,
                NewFeedback = new Feedback { Product_Id = (int)id }

            };

            unit.save();
            return View(feedbackVM);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Feedback(Feedback feedback)
        {

            feedback.Id = 0;
            if (feedback.Comment != null) { 
           
                var user_claims = (ClaimsIdentity)User.Identity;
                var user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = unit.User.GetByFilter(u => u.Id == user_id);
                feedback.UserName = user.Name;
                feedback.Profile_image = user.ProfilePicture;
                feedback.User_Id = user.Id;
                feedback.CreatedAt = DateTime.Now;
                unit.Feedback.Create(feedback);
                unit.save();
                TempData["success"] = "Your feedback has been added!";

                return RedirectToAction("Feedback", new { id = feedback.Product_Id });
            }

            return RedirectToAction("Feedback", new { id = feedback.Product_Id });
        }


        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var feedback = unit.Feedback.GetByFilter(f => f.Id == id);
            if (feedback == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (feedback.User_Id != userId) return Forbid();

            unit.Feedback.Delete(feedback);
            unit.save();
            TempData["success"] = "Your comment has been deleted!";
            return RedirectToAction("Feedback", new { id = feedback.Product_Id });
        }


    }

}


