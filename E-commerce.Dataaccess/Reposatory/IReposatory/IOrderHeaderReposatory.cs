using E_commerce.Model.Models;
using E_commerce_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory.IReposatory
{
    public interface IOrderHeaderReposatory:IReposatory<OrderHeader>
    {

        public void UpdateStatus(int id, string OrderStatus, string? Paymentstatus=null);
		public void UpdateStripePaymentID(int id, string SessionId, string Paymentintentid);
	}
}
