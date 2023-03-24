using IdentityBlogApp.Web.Areas.Admin.Models;
using IdentityBlogApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityBlogApp.Web.Areas.Admin.Controllers
{
        [Area("Admin")]
        [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var userList=await _userManager.Users.ToListAsync();
            var userModelList = userList.Select(x => new UserViewModel { Id = x.Id, Email = x.Email, Name = x.UserName }).ToList();
            return View(userModelList);
        }
    }
}
