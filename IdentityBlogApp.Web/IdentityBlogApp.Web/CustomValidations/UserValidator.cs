using IdentityBlogApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isNumeric = int.TryParse(user.UserName[0]!.ToString(), out _);
            if (isNumeric)
            {
                errors.Add(new IdentityError() { Code = "UserNameWithStartDigit", Description = "Kullanıcı adı numara ile başlayamaz." });
            }
            if (user.UserName.ToLower().Contains("admin"))
            {
                errors.Add(new IdentityError() { Code = "UserNameContainsAdmin", Description = "Kullanıcı adı Admin kelimesini içeremez." });
            } 
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
