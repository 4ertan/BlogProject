using Microsoft.AspNetCore.Identity;

namespace IdentityBlogApp.Web.Models
{
    public class AppUser:IdentityUser
    {
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte? Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public AppUser()
        {
            this.CreatedDate = DateTime.Now;
        }

    }
}
