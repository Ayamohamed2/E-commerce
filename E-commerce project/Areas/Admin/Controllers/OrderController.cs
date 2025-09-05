using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using E_commerce.Model.ViewModels;
using System.Security.Claims;
using Stripe;
using Stripe.Checkout;
using E_commerce.Dataaccess.Reposatory;
namespace E_commerce_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unit;
      Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;


        public OrderController(IUnitOfWork unit, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            this.unit = unit;
            this.userManager = userManager;
               
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Details(int id)
        {
            OrderVM orderVM = new()
            {
                OrderHeader = unit.OrderHeader.GetByFilter(o => o.Id == id,Includes:"ApplicationUser"),
                OrderDetails=unit.OrderDetails.GetALL(o=>o.OrderHeaderId==id,Includes:"Product")
            };

            return View(orderVM);
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateDetail(OrderVM orderVM)
        {
            var order = unit.OrderHeader.GetByFilter(o => o.Id == orderVM.OrderHeader.Id);
            order.Name = orderVM.OrderHeader.Name;
            order.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
            order.StreetAddress = orderVM.OrderHeader.StreetAddress;
            order.City = orderVM.OrderHeader.City;
            order.State = orderVM.OrderHeader.State;
            order.PostalCode = orderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(orderVM.OrderHeader.Carrier))
            {
                order.Carrier = orderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
            {
                order.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }
            unit.OrderHeader.Update(order);
            unit.save();
            TempData["success"] = "Order updated successfully";

            return RedirectToAction("details", new { id = order.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(OrderVM orderVM)
        {
           

            unit.OrderHeader.UpdateStatus(orderVM.OrderHeader.Id, SD.StatusInProcess);

           
            unit.save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("details", new { id = orderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder(OrderVM orderVM)
        {
            var order = unit.OrderHeader.GetByFilter(o => o.Id == orderVM.OrderHeader.Id);
            order.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            order.Carrier = orderVM.OrderHeader.Carrier;

            order.OrderStatus = SD.StatusShipped;
            order.ShippingDate = DateTime.Now;
            if (order.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                order.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            unit.OrderHeader.Update(order);
            unit.save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction("details", new { id = order.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
			var order = unit.OrderHeader.GetByFilter(o => o.Id == orderVM.OrderHeader.Id);
            if (order.PaymentStatus == SD.PaymentStatusApproved)
            {
                var option = new RefundCreateOptions
                {
                    Reason=RefundReasons.RequestedByCustomer,
                    PaymentIntent=order.PaymentIntentId

                };

                var service = new RefundService();
                var refunded = service.Create(option);

                unit.OrderHeader.UpdateStatus(order.Id, SD.StatusCancelled, SD.StatusRefunded);

            }
            else
            {
				unit.OrderHeader.UpdateStatus(order.Id, SD.StatusCancelled, SD.StatusCancelled);
			}
            unit.save();
			TempData["Success"] = "Order Cancelled Successfully.";
			return RedirectToAction(nameof(Details), new { id = order.Id });

		}

		[ActionName("Details")]
		[HttpPost]
		public IActionResult Details_PAY_NOW(OrderVM orderVM)
        {

            orderVM.OrderHeader = unit.OrderHeader.GetByFilter(o => o.Id == orderVM.OrderHeader.Id,Includes:"ApplicationUser");
            orderVM.OrderDetails = unit.OrderDetails.GetALL(o => o.OrderHeader.Id == orderVM.OrderHeader.Id,Includes:"Product");
			string domain = "http://localhost:5272/";
			var options = new Stripe.Checkout.SessionCreateOptions
			{
				SuccessUrl = domain + $"admin/order/PaymentConfirmation/{orderVM.OrderHeader.Id}/",
				CancelUrl = domain + "admin/order/details/"+orderVM.OrderHeader.Id ,

				LineItems = new List<SessionLineItemOptions>(),

				Mode = "payment",
			};
			foreach (var o in orderVM.OrderDetails)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(o.Price * 100),
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = o.Product.title
						}
					},
					Quantity = o.Count

				};
				options.LineItems.Add(sessionLineItem);

			}
			var service = new Stripe.Checkout.SessionService();
			Stripe.Checkout.Session session = service.Create(options);
			unit.OrderHeader.UpdateStripePaymentID(orderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
			unit.save();

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);

		}

        public IActionResult PaymentConfirmation(int id)
        {
            var order = unit.OrderHeader.GetByFilter(o => o.Id == id, Includes: "ApplicationUser");
            if (order.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(order.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    unit.OrderHeader.UpdateStripePaymentID(order.Id, session.Id, session.PaymentIntentId);
                    unit.OrderHeader.UpdateStatus(order.Id, order.OrderStatus, SD.PaymentStatusApproved);
                    unit.save();
                }

            }

            return View(id);

        }
        public async Task<IActionResult> GetAll(string status)
        {
            IEnumerable<OrderHeader> orders ;

            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orders = unit.OrderHeader.GetALL(Includes: "ApplicationUser");
            }
            else
            {


                var user_claims = (ClaimsIdentity)User.Identity;
                string user_id = user_claims.FindFirst(ClaimTypes.NameIdentifier).Value;

                orders = unit.OrderHeader.GetALL(o => o.User_Id == user_id, Includes: "ApplicationUser");
            }
            if(status== "pending")
            {
                orders = orders.Where(o => o.PaymentStatus == SD.PaymentStatusDelayedPayment);

            }
            else if(status== "inprocess")
            {
                orders= orders.Where(o => o.OrderStatus == SD.StatusInProcess);

            }
            else if(status== "completed")
            {
                orders = orders.Where(o => o.OrderStatus == SD.StatusShipped);
            }
            else if(status== "approved")
            {
                orders = orders.Where(o => o.OrderStatus == SD.StatusApproved);
            }
            
            foreach(var o in orders)
            {
                var roles = await userManager.GetRolesAsync(o.ApplicationUser);
                if (roles.FirstOrDefault() == "Company")
                {
                    o.Name = o.Name + "(Company)";
                }
            }

            
        
            return Json(new { data = orders });

        }
    }
}
