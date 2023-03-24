using IdentityBlogApp.Web.Extenisons;
using IdentityBlogApp.Web.Models;
using IdentityBlogApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityBlogApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserViewModel
            {
                Email = currentUser!.Email,
                PhoneNumber = currentUser.PhoneNumber,
                UserName = currentUser.UserName
            };
            return View(userViewModel);
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList =new SelectList(Enum.GetNames(typeof(GenderEnum)));

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            UserEditViewModel userEditViewModel = new UserEditViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Phone = user.PhoneNumber,
                Gender = user.Gender,

            };

            return View(userEditViewModel);
        }
        public IActionResult AccessDenied(string Url)
        {
            return View();

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Geçerli şifre yanlış");
                return View();
            }
            var resultChange = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);
            if (!resultChange.Succeeded)
            {
                ModelState.AddModelErrorList(resultChange.Errors.Select(x => x.Description).ToList());
                return View();
            }
            //Cookie yenilemek için
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, true);
            TempData["SuccessMessage"] = "Şifre değiştirme işlemi başarılı";
            return View();
            
        }
    }
}
