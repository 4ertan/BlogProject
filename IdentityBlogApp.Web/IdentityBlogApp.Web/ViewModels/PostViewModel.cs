using IdentityBlogApp.Web.Models;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.ViewModels
{
    public class PostViewModel
    {
        public int? Id { get; set; }
        [MinLength(3,ErrorMessage ="Lütfen daha uzun yazınız.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Başlık alanı boş geçilemez")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Bu alan boş geçilemez")]
        [Display(Name ="İcerik")]
        [MinLength(25,ErrorMessage ="Minimum 25 karakter giriniz")]
        public string Body { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Bu alan boş gecilemez")]
        public string ImageUrl { get; set; }
        public long ClickCount { get; set; }
        public DateTime CreatedDate { get; set; }
     
        public AppUser? Author { get; set; }
        public List<Tag>? Tags { get; set; }
      


    }
}
