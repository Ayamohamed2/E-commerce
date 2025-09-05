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
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryReposatory Category { get; private set; }
        public IProductReposatory Product { get; private set; }
        public IShoppingCartReposatory ShoppingCart { get; private set; }
        public ICompanyReposatory Company { get; private set; }
		public IOrderDetailsReposatory OrderDetails { get; private set; }

		public IOrderHeaderReposatory OrderHeader { get; private set; }
		public IApplicationUserReposatory User { get; private set; }

        public IProductImageReposatory ProductImage { get; private set; }

        public IFeedbackReposatory Feedback { get; private set; }
        private Context context;
        public UnitOfWork(Context context)
        {
            this.context = context;
            Category = new CategoryReposatory(context);
            Product = new ProductReposatory(context);
            Company = new CompanyReposatory(context);
            ShoppingCart = new ShoppingCartReposatory(context);
            User = new ApplicationUserReposatory(context);
            OrderHeader = new OrderHeadrReposatory(context);
            OrderDetails = new OrderDetailsReposatory(context);

            ProductImage = new ProductImageReposatory(context);

            Feedback = new FeedbackReposatory(context);
        }
        public void save()
        {
            context.SaveChanges();
        }
    }
}
