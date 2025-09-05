using E_commerce_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Model.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string Discription { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name ="List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1,1000)]
        [Display(Name = "Price for 1-50")]
        public double price { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 50+")]
        public double price50 { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 100+")]
        public double price100 { get; set; }

        [ForeignKey("Category")]
        public int Category_id { get; set; }

        public Category? Category { get; set; }

        [ValidateNever]
        public List<ProductImage>? ProductImages { get; set; }

        [NotMapped]
        public List<IFormFile>? files { get; set; }

    }
}
