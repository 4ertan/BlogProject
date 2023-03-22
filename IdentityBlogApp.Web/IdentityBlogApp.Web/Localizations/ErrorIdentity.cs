using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.Localizations
{
    public class ErrorIdentity:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"Bu kullanıcı adı ({userName}) başka bir kullanıcı tarafından kullanılmaktadır." };
            //return base.DuplicateUserName(userName);
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError() { Code = "DuplicateEmail", Description = $"Bu email({email}) başka bir kullanıcı tarafından kullanılmaktadır." };
            //return base.DuplicateEmail(email);
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError() { Code = "PasswordTooShort", Description = "Şifre en az 6 karakterden oluşmalıdır." };
            //return base.PasswordTooShort(length);
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError() { Code = "PasswordRequiresUpper", Description = "Şifre en az 1 adet büyük karakter içermelidir." };
            //return base.PasswordRequiresUpper();
        }
        //public override IdentityError PasswordRequiresNonAlphanumeric()
        //{
        //    return new IdentityError() { Code = "PasswordRequiresNonAlphanumeric", Description = "Şifre özel karakter içermelidir." };
        //    //return base.PasswordRequiresNonAlphanumeric();
        //}
    }
}
