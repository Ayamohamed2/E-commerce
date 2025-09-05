using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Model.Models
{
    public class Feedback
    {
        
        public int Id { get; set; }

       
        public string? User_Id { get; set; }

        
        public string? UserName { get; set; } 

        public string? Profile_image { get; set; } 

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }

        [Required]
   
        public string Comment { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
