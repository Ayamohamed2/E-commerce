using E_commerce.Data;
using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory
{
    public class OrderDetailsReposatory : Reposatory<OrderDetails>, IOrderDetailsReposatory


    {
        Context context;

        public OrderDetailsReposatory(Context context) : base(context)
        {
            this.context = context;
        }


    }
}
