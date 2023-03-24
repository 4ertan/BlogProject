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
            _appDbContext.Tag.Add(addTag);
            _appDbContext.SaveChanges();
            TempData["Success"] = $"Tag {model.Name} eklendi";
            return RedirectToAction("AddTag");



        }
        public IActionResult TagList()
        {
           var tagList=_appDbContext.Tag.ToList();
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
