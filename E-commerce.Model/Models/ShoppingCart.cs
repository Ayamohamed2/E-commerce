using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Model.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Range(1,1000,ErrorMessage ="Please enter value between 1 and 1000")]
        public int count { get; set; }
        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        [ValidateNever]
        public Product? Product { get; set; }
        [ForeignKey("User")]
        public string User_Id { get; set; }
        [ValidateNever]
        public ApplicationUser? User { get; set; }

        [NotMapped]
        public double price { get; set; }
    }
}
