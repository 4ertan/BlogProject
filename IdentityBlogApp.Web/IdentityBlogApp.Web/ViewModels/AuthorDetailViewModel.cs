using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.ViewModels
{
    public class AuthorDetailViewModel
    {
        public string Id { get; set; }
        [Display(Name ="Kullanıcı Adı")]
        public string? UserName { get; set; }
        [Display(Name = "Hesap Oluşturma Tarihi")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Toplam Post Sayısı")]
        public int TotalPost { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }

    }
}
