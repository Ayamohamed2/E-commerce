using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory.IReposatory
{
    public interface IUnitOfWork
    {
        public ICategoryReposatory Category  { get; }
        public IProductReposatory Product { get; }
        public IShoppingCartReposatory ShoppingCart { get; }
        public ICompanyReposatory Company { get;  }
        public IApplicationUserReposatory User { get; }

		public IOrderHeaderReposatory OrderHeader { get; }

		public IOrderDetailsReposatory OrderDetails { get; }

        public IProductImageReposatory ProductImage { get; }

        public IFeedbackReposatory Feedback { get; }
        void save();
    }
}
