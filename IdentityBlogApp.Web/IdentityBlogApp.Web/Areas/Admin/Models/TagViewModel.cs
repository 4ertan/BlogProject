using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace IdentityBlogApp.Web.Areas.Admin.Models
{
    public class TagViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tag alanı boş bırakılamaz")]
        [Display(Name ="Etiket Adı")]
        public string Name { get; set; }
    
    }
}
