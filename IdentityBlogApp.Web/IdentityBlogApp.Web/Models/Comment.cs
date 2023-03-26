using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityBlogApp.Web.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int PostId { get; set; }
        public Post Post { get; set; }
     
        public string UserId{ get; set; }  

       
    }
}
