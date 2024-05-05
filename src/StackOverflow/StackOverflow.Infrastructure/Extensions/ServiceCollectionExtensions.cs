using FirstDemo.Infrastructure.Membership;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StackOverflow.Infrastructure.DbContexts;
using StackOverflow.Infrastructure.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(o => o.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddRazorPages();
        }

        public static void AddCookieAuthentication(this IServiceCollection services)
        {
            // services.AddAuthentication()
            //     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //     {
            //         options.LoginPath = new PathString("/Auth/Login");
            //         options.AccessDeniedPath = new PathString("/Auth/Login");
            //         options.LogoutPath = new PathString("/Auth/Logout");
            //         options.Cookie.Name = "Stackoverflow.Identity";
            //         options.SlidingExpiration = true;
            //         options.ExpireTimeSpan = TimeSpan.FromHours(1);
            //     });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Auth/Login");
                options.AccessDeniedPath = new PathString("/Auth/Login");
                options.LogoutPath = new PathString("/Auth/Logout");
                options.Cookie.Name = "Stackoverflow.Identity";
                //resets the cookie's expiration time each time the cookie is sent to the server
                options.SlidingExpiration = true;
                //sets the cookie's expiration time to 1 hour
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });
        }

        //public static void AddJwtAuthentication(this IServiceCollection services,
        //    string key, string issuer, string audience)
        //{
        //    services.AddAuthentication()
        //        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
        //        {
        //            x.RequireHttpsMetadata = false;
        //            x.SaveToken = true;
        //            x.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        //                ValidateIssuer = true,
        //                ValidateAudience = true,
        //                ValidIssuer = issuer,
        //                ValidAudience = audience,
        //            };
        //        });
        //}
    }
}
