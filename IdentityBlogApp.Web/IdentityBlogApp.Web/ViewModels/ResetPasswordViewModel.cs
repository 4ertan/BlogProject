using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }
    }
}
