using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Eski Şifre alanı boş bırakılamaz")]
        [Display(Name = "Eski Şifre :")]
        public string PasswordOld { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [Display(Name = "Şifre :")]
        public string PasswordNew { get; set; }

        [Compare(nameof(PasswordNew), ErrorMessage = "Şifre aynı değildir.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Tekrar alanı boş bırakılamaz")]
        [Display(Name = "Şifre Tekrar :")]
        public string PasswordConfirm { get; set; }

    }
}
