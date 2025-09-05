using E_commerce.Data;
using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce_project.Migrations;
using E_commerce_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory
{
	public class OrderHeadrReposatory : Reposatory<OrderHeader>, IOrderHeaderReposatory
	{
		Context Context;
		public OrderHeadrReposatory(Context context) : base(context)
		{
			this.Context = context;
		}

		public void UpdateStatus(int id, string OrderStatus, string? Paymentstatus = null)
		{
			var order = Context.OrderHeader.FirstOrDefault(o => o.Id == id);
			if (order != null)
			{
				order.OrderStatus = OrderStatus;

				if (!string.IsNullOrEmpty(Paymentstatus))
				{
					order.PaymentStatus = Paymentstatus;
				}
			}
		}

		public void UpdateStripePaymentID(int id, string SessionId, string Paymentintentid)
		{
			var order = Context.OrderHeader.FirstOrDefault(o => o.Id == id);
			if (order != null)
			{
				if (!string.IsNullOrEmpty(SessionId))
				{
					order.SessionId = SessionId;
				}
				if (!string.IsNullOrEmpty(Paymentintentid))
				{
					order.PaymentIntentId = Paymentintentid;
					order.PaymentDate = DateTime.Now;
				}

			}
		}
	}
}
