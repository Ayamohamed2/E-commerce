using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce.Model.ViewModels;
using E_commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace E_commerce_project.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        ShoppingCartVM ShoppingCartVM;
        private readonly IUnitOfWork unit;
        private readonly IEmailSender emailSender;

        public ShoppingCartController(IUnitOfWork unit,IEmailSender emailSender)
        {
            this.unit = unit;
            this.emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var user_claims = (ClaimsIdentity)User.Identity;
            string user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCarts = unit.ShoppingCart.GetALL(u => u.User_Id == user_id, Includes: "Product")
               ,
                OrderHeader = new()

            };

            foreach(var cart in ShoppingCartVM.ShoppingCarts)
            {
                cart.Product.ProductImages = unit.ProductImage.GetALL(p => p.Product_Id == cart.Product.Id);
                 cart.price = GetPriceBasedOnQuantiy(cart) ;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.count);
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cart_id)
        {
            var cart = unit.ShoppingCart.GetByFilter(c => c.Id == cart_id);

            cart.count += 1;
            unit.ShoppingCart.Update(cart);
            unit.save();
            return RedirectToAction("index");
        }


        public IActionResult Minus(int cart_id)
        {
            var cart = unit.ShoppingCart.GetByFilter(c => c.Id == cart_id);
            if (cart.count <= 1)
            {
                unit.ShoppingCart.Delete(cart);
            }
            else
            {

                cart.count -= 1;
                unit.ShoppingCart.Update(cart);
            }
            unit.save();
            return RedirectToAction("index");
        }


        public IActionResult Remove(int cart_id)
        {
            var cart = unit.ShoppingCart.GetByFilter(c => c.Id == cart_id);

            unit.ShoppingCart.Delete(cart);
            unit.save();
            return RedirectToAction("index");
        }

        public IActionResult Summary()
        {

            var user_claims = (ClaimsIdentity)User.Identity;
            string user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCarts = unit.ShoppingCart.GetALL(u => u.User_Id == user_id, Includes: "Product")
               ,
                OrderHeader = new()

            };
            ShoppingCartVM.OrderHeader.ApplicationUser = unit.User.GetByFilter(u => u.Id == user_id);


            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.state;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;


            foreach (var cart in ShoppingCartVM.ShoppingCarts)
            {
                cart.price = GetPriceBasedOnQuantiy(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.count);
            }
            return View(ShoppingCartVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Summary(ShoppingCartVM ShoppingCartVM)
		{
           

                var user_claims = (ClaimsIdentity)User.Identity;
                string user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                ShoppingCartVM.ShoppingCarts = unit.ShoppingCart.GetALL(u => u.User_Id == user_id, Includes: "Product");
				ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
				ShoppingCartVM.OrderHeader.User_Id = user_id;
               
             ApplicationUser ApplicationUser = unit.User.GetByFilter(u => u.Id == user_id);

				foreach (var cart in ShoppingCartVM.ShoppingCarts)
				{
					cart.price = GetPriceBasedOnQuantiy(cart);
					ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.count);
				}
			if (ModelState.IsValid)
			{
				if (ApplicationUser.Company_Id.GetValueOrDefault() == 0)
                {
                    //customer 
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;


                }
                else
                {
                    //company
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;

                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;

                }

                unit.OrderHeader.Create(ShoppingCartVM.OrderHeader);
                unit.save();

                foreach (var cart in ShoppingCartVM.ShoppingCarts)
                {
                    OrderDetails orderDetails = new()
                    {
                        ProductId = cart.Product_Id,
                        OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                        Price = cart.price,
                        Count = cart.count
                    };

                    unit.OrderDetails.Create(orderDetails);
                    unit.save();
                }


                if(ApplicationUser.Company_Id.GetValueOrDefault() == 0)
                {
                    // customer payment stripe


                    string domain = "http://localhost:5272/";
                    var options = new Stripe.Checkout.SessionCreateOptions
                    {
                        SuccessUrl = domain+ $"customer/shoppingcart/OrderConfirmation/{ShoppingCartVM.OrderHeader.Id}/",
                        CancelUrl=domain+ "customer/shoppingcart/index",

						LineItems = new List<SessionLineItemOptions>(),

						Mode = "payment",
                    };
                    foreach (var cart in ShoppingCartVM.ShoppingCarts)
                    {
                        var sessionLineItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(cart.price * 100),
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = cart.Product.title
                                }
                            },
                            Quantity=cart.count

                        };
                        options.LineItems.Add(sessionLineItem);

					}
					var service = new Stripe.Checkout.SessionService();
					Stripe.Checkout.Session session = service.Create(options);
					unit.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                    unit.save();

                    Response.Headers.Add("Location", session.Url);

                  
                    return new StatusCodeResult(303);
				}

                return RedirectToAction("OrderConfirmation", new {id= ShoppingCartVM.OrderHeader.Id });
              
            }


			
			return View(ShoppingCartVM);
			
		}



        public IActionResult OrderConfirmation(int id)
        {

            var order = unit.OrderHeader.GetByFilter(o=>o.Id==id,Includes: "ApplicationUser");
            if (order.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
				//customer

				var service = new SessionService();
                var session = service.Get(order.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    //success

                   

                    unit.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    unit.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    unit.save();
                }
            }

            emailSender.SendEmailAsync(order.ApplicationUser.Email, "New Order - E-Book", $"<p>New Order Created - {order.Id}</p>");

            List<ShoppingCart> carts = unit.ShoppingCart.GetALL(u => u.User_Id == order.User_Id);

            unit.ShoppingCart.RemoveRange(carts);
            unit.save();
            return View(id);
        }
		private double GetPriceBasedOnQuantiy(ShoppingCart cart)
        {
            if (cart.count <= 50)
            {
                return cart.Product.price;
            }
            else
            {
                if (cart.count <= 100) return cart.Product.price50;
                else return cart.Product.price100;

            }
        }
    }
}
