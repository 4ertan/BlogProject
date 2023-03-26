using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.Models
{
    public class Post
    {
        public Post()
        {
            ClickCount = 0;
        }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public long ClickCount { get; set; } 
     
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public ICollection<Tag> PostTags { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
