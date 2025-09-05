using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Model.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? state { get; set; }
        public string? PostalCode { get; set; }

        [ForeignKey("Company")]
        public int? Company_Id { get; set; }
        [ValidateNever]
        public Company? Company { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public string? ProfilePicture { get; set; } = "/Images/default.png";
    }
}
