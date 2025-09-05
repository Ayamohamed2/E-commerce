using E_commerce.Data;
using E_commerce.Model.Models;
using E_commerce.Model.ViewModels;
using E_commerce.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using E_commerce_project.Migrations;
namespace E_commerce_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly Context context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> manager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager;
        public UserController(Context context, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> manager)
        {
            this.context = context;
            this.manager = manager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RoleManagment( string userId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) return NotFound();
            var UserRole = context.UserRoles.ToList();
            var Role = context.Roles.ToList();
            var role_id = UserRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            var role_name = Role.FirstOrDefault(r => r.Id == role_id).Name;
            user.Role = role_name;

            var Roles = context.Roles;
           var Companies = context.Companies.ToList();

            var userVM = new UserVM
            {
                User = user,
                CompanyList= Companies,
                RoleList =Roles

            };
            return View(userVM);

        }

        [HttpPost]

        public async Task<IActionResult> RoleManagment(string userId, UserVM userVM)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (userVM.User.Role == SD.Role_Company)
            {
                user.Company_Id = userVM.User.Company_Id;
            }
            else
            {
                user.Company_Id = null;
            }


            var Role = await manager.GetRolesAsync(user);
            await manager.RemoveFromRolesAsync(user, Role);
            if (!string.IsNullOrEmpty(userVM.User.Role))
            {
                await manager.AddToRoleAsync(user, userVM.User.Role);
            }
            else
            {
                await manager.AddToRoleAsync(user, SD.Role_Customer);
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult GetAll()
        {
            var users = context.Users.Include("Company").ToList();

            var UserRole = context.UserRoles.ToList();
            var Roles = context.Roles.ToList();

            foreach(var user in users)
            {
                var role_id = UserRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                var role_name = Roles.FirstOrDefault(r => r.Id == role_id).Name;
                user.Role = role_name;
            }
            return Json(new { data = users });

        }

        public IActionResult LockUnlock([FromBody]string id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
				return Json(new { success = false, Message = "User not found" });
			}

            if (user.LockoutEnd != null && user.LockoutEnd>DateTime.Now)//lock
            {
                //unlock

                user.LockoutEnd = DateTime.Now;

            }
            else
            {
                //lock
                user.LockoutEnd = DateTime.Now.AddYears(1000);

			}

            context.SaveChanges();

			return Json(new { success = true, Message = "Operation Successful" });
		}
    }
}
