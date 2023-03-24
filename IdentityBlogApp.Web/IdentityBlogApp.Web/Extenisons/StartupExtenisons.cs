using IdentityBlogApp.Web.CustomValidations;
using IdentityBlogApp.Web.Localizations;
using IdentityBlogApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.Extenisons
{
    public static class StartupExtenisons
    {
        public static void AddIdentityWithExt(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_ABCDEFGHIJKLMNOPRSTUVWYZ";
                
                
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddPasswordValidator<PasswordValidator>()
              .AddErrorDescriber<ErrorIdentity>()
              .AddUserValidator<UserValidator>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
        }
    }
}
