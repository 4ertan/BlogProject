using IdentityBlogApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı alanı boş bırakılamaz")]
        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = null!;
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi :")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Profil Fotosu :")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Cinsiyet :")]
        public GenderEnum? Gender { get; set; }

        public string Bio { get; set; }
    }
}
