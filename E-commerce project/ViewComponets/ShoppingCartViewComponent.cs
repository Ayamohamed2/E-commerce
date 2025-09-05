using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_project.ViewComponant
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unit;

        public ShoppingCartViewComponent(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user_claims = (ClaimsIdentity)User.Identity;
            var user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier);
            if (user_id != null)//log in
            {
                
                    HttpContext.Session.SetInt32(SD.SessionCart, unit.ShoppingCart.GetALL(u => u.User_Id == user_id.Value).Count);


                
                return View(HttpContext.Session.GetInt32(SD.SessionCart));

            }
            else 
            {
                HttpContext.Session.Clear();
                return View(0);

            }
        }
    }
}
