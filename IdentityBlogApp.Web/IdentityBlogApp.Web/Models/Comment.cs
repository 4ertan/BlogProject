using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
