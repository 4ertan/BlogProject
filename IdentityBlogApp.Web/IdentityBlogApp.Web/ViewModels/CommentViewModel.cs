using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Yorum")]
        public string Text { get; set; }
        
    }
}
