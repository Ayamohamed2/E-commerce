using E_commerce.Dataaccess.Reposatory;
using E_commerce.Dataaccess.Reposatory.IReposatory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using E_commerce.Data;
using E_commerce.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using E_commerce.Model.Models;
using Stripe;
using E_commerce.Dataaccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>(optionbuilder =>
{
    optionbuilder.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});

builder.Services.Configure<StripeSittings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    
    options => {

        options.SignIn.RequireConfirmedAccount = true;

        options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(1);

        options.Lockout.MaxFailedAccessAttempts = 3;

        options.Lockout.AllowedForNewUsers = true;
    }




).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(
    options=>
    {
        options.LoginPath = $"/Identity/Account/Login";
        options.LogoutPath = $"/Identity/Account/Logout";
        options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

    }
    );


builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1486964735779677";
   
}).AddGoogle(
    options =>
     {
         IConfigurationSection googlAuth = builder.Configuration.GetSection("Authentication:Google");
         options.ClientId = googlAuth["Clientid"];
         
     }
     );

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}