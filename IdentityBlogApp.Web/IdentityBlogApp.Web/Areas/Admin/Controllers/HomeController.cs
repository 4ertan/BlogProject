using IdentityBlogApp.Web.Areas.Admin.Models;
using IdentityBlogApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityBlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var postList=_appDbContext.Posts.ToList();
            return View(postList);
        }
        public IActionResult PostList()
        {
            return View();
        }
        public IActionResult AddTag()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTag(TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Tag oluşturulamadı.");
                return View();
            }
            Tag addTag = new Tag() { Name = model.Name };
            _appDbContext.Tags.Add(addTag);
            _appDbContext.SaveChanges();
            TempData["Success"] = $"Tag {model.Name} eklendi";
            return RedirectToAction("AddTag");

        }
        public IActionResult UserDelete(string id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Hatalı işlem");
                return RedirectToAction("UserList");
            }
            var user=_appDbContext.Users.Find(id);
            UserViewModel model = new() { Id=user.Id, Name=user.UserName,CreatedDate=user.CreatedDate,Email=user.Email };
            return View(model);
        }
        public IActionResult TagList()
        {
           var tagList=_appDbContext.Tags.ToList();
            return View(tagList);
        }
        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            var userModelList = userList.Select(x => new UserViewModel { Id = x.Id, Email = x.Email, Name = x.UserName }).ToList();
            return View(userModelList);
        }
    }
}
