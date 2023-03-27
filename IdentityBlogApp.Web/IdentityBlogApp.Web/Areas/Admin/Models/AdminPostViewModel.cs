namespace IdentityBlogApp.Web.Areas.Admin.Models
{
    public class AdminPostViewModel
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ClickCount { get; set; }
        public string Title { get; set; }
    }
}
