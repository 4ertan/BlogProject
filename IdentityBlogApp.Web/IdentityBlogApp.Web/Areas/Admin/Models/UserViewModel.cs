using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.Areas.Admin.Models
{
    public class UserViewModel
    {

        public string? Id { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        public string? Name { get; set; }
        [Display(Name = "Email Adresi")]
        public string? Email { get; set; }
        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedDate { get; set; }
    }
}
