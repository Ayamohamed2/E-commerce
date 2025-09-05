using E_commerce.Data;
using E_commerce.Model.Models;
using E_commerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> user;
        private readonly RoleManager<IdentityRole> role;
        private readonly Context context;

        public DbInitializer(UserManager<ApplicationUser>user,RoleManager<IdentityRole>role,Context context)
        {
            this.user = user;
            this.role = role;
            this.context = context;
        }
      
        public void Initialize()
        {
            try
            {
                if (context.Database.GetPendingMigrations().Count() > 0)
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            if (!role.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                role.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                role.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                role.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                role.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                user.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                    StreetAddress="Cairo",
                    state="Egypt",
                    City="Cairo",
                    PostalCode="1233",
                    Name="Aya",
                    PhoneNumber= "1112223333"
                },"Admin2004*").GetAwaiter().GetResult();

                var app_user = context.Users.FirstOrDefault(u => u.Email == "Admin@gmail.com");
                user.AddToRoleAsync(app_user, SD.Role_Admin).GetAwaiter().GetResult();

            }
            return;

        }
    }
}
