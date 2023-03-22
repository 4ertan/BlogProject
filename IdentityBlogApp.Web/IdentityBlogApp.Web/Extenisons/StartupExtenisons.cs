﻿using IdentityBlogApp.Web.CustomValidations;
using IdentityBlogApp.Web.Localizations;
using IdentityBlogApp.Web.Models;

namespace IdentityBlogApp.Web.Extenisons
{
    public static class StartupExtenisons
    {
        public static void AddIdentityWithExt(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddPasswordValidator<PasswordValidator>().AddErrorDescriber<ErrorIdentity>().AddUserValidator<UserValidator>().AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
