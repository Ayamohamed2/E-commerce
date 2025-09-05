// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_commerce_project.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment env;
        private readonly IUnitOfWork unit;
        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,  IWebHostEnvironment env, IUnitOfWork unit)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.env = env;
            this.unit = unit;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


            [Required]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

        

           

            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "State")]
            public string State { get; set; }

            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile ProfilePictureFile { get; set; }

            public string? imageProfile { get; set; }

            public string Role { get; set; }
            [ValidateNever]
            public string? CompanyName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            Username = userName;
            string? company = null;
            if (User.IsInRole(SD.Role_Company) )
                 company = unit.Company.GetByFilter(c => c.id == user.Company_Id).Name;
            
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,

                Name = user.Name,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.state,
                PostalCode = user.PostalCode,
                imageProfile = user.ProfilePicture,
                Role = roles.FirstOrDefault(),
                CompanyName = company
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            user.Name = Input.Name;
            user.StreetAddress = Input.StreetAddress;
            user.City = Input.City;
            user.state = Input.State;
            user.PostalCode = Input.PostalCode;
            if (Input.ProfilePictureFile != null && Input.ProfilePictureFile.Length > 0)
            {
                user.ProfilePicture = GetImageURL(Input.ProfilePictureFile);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }


        public string GetImageURL(IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return null;
            }

            string folderpath = Path.Combine(env.WebRootPath, "Images/Profile");
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            string path = Path.Combine(folderpath, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }
            return "/Images/Profile/" + fileName;

        }
    }
}
