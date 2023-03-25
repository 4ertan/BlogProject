using IdentityBlogApp.Web.Models;

namespace IdentityBlogApp.Web.ViewModels
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public List<Tag> Tags { get; set; }
      

    }
}
