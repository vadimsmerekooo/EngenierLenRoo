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
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<MainContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MainContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZàáâãäå¸æçèéêëìíîïğñòóôõö÷øùúûüışÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÚÛÜİŞß0123456789-._@+ ";
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MainContext>();
                //services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<MainContext>();


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