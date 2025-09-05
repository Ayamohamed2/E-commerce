using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace E_commerce_project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unit;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unit)
        {
            _logger = logger;
            this.unit = unit;
        }

        public IActionResult Index()
        {


            var products = unit.Product.GetALL(Includes: "Category,ProductImages");
            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int? id)

        {
            var Product_Id = id;
            if (Product_Id == 0 || Product_Id == null) return NotFound();

            ShoppingCart cart = new ShoppingCart()
            {
                Product = unit.Product.GetByFilter(p => p.Id == Product_Id, Includes: "Category,ProductImages"),
                count = 1,
                Product_Id =(int)Product_Id

            };
           
            return View(cart);
        }

        [HttpPost]
        
        [Authorize]
        public IActionResult Details(ShoppingCart Cart)
        {
            Cart.Id = 0;

            var user_claims = (ClaimsIdentity)User.Identity;
            string user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            Cart.User_Id = user_id;

            var model = unit.ShoppingCart.GetByFilterAsnoTraking(c => c.User_Id == user_id && c.Product_Id == Cart.Product_Id);
            if (model != null)
            {
                model.count += Cart.count;
                unit.ShoppingCart.Update(model);

            }
            else
            {
                unit.ShoppingCart.Create(Cart);
            }
            unit.save();

            TempData["success"] = "Product added to Cart successfully";
            return RedirectToAction("index");

        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
