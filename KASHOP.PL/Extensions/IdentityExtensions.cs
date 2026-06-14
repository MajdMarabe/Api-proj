using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.PL.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services) {

            Services.AddIdentity<ApplicationUser, IdentityRole>(
              Options =>
              {
                  Options.User.RequireUniqueEmail = true;
                  ///password policy
                  Options.Password.RequireDigit = true;
                  Options.Password.RequireLowercase = true;
                  Options.Password.RequireUppercase = true;
                  Options.Password.RequireNonAlphanumeric = true;
                  //  Options.Password.RequiredLength = 10;
                  //
                  Options.Lockout.MaxFailedAccessAttempts = 5;
                  Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
              }

              )
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
            return Services;




        }
    }
}
