using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Data;
namespace E_commerce.Dataaccess.Reposatory
{
    public class ProductReposatory:Reposatory<Product>,IProductReposatory
    {
        private readonly Context context;
        
        public ProductReposatory(Context context):base(context)
        {
            this.context = context;
        }
     

    }
}
