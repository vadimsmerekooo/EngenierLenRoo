using System;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(EngeneerLenRooAspNet.Areas.Identity.IdentityHostingStartup))]
namespace EngeneerLenRooAspNet.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MainContext>(options =>
                    options.UseSqlServer("Server=82.209.211.198;Database=EngeneerLenRoo;User Id=programmerlenroo; Password=Qwerty123456!"));

                services.AddDefaultIdentity<IdentityUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequiredLength = 5;  
                        options.Password.RequireNonAlphanumeric = false;   
                        options.Password.RequireLowercase = false; 
                        options.Password.RequireUppercase = false; 
                        options.Password.RequireDigit = false; 
                        
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MainContext>();
                services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/access-denied";
                    options.Cookie.Name = "lenrooengprog";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(4320);
                    options.LoginPath = "/account/signin";
                    options.SlidingExpiration = true;
                });
                services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                    opt =>
                    {
                        opt.LoginPath = "/account/signin";
                    });
            });
        }
    }
}