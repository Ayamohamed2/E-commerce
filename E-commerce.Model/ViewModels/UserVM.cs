using E_commerce.Model.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;

using Microsoft.AspNetCore.Identity;

namespace E_commerce.Model.ViewModels
{
    public class UserVM
    {
        public ApplicationUser User { get; set; }
        [ValidateNever]
        public IEnumerable<IdentityRole> RoleList { get; set; }
        [ValidateNever]
        public IEnumerable<Company> CompanyList { get; set; }
    }
}
