using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.Models
{
    public class AppUser:IdentityUser
    {
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;

        public string? Bio { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }

    }
}
