using IdentityBlogApp.Web.Areas.Admin.Models;
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
        private readonly AppDbContext _appDbContext;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new ViewModels.UserViewModel
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

        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
            return View();

            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            currentUser.UserName= model.UserName;
            currentUser.Email= model.Email;
            currentUser.Gender= model.Gender;
            currentUser.Bio=model.Bio;

            await _userManager.UpdateAsync(currentUser);


            return View();

        }
        public async Task<IActionResult> PostAdd()
        {
            if (_appDbContext.Tags.ToList()!=null)
            {
                var tagList = _appDbContext.Tags.ToList();
                ViewBag.tagList = tagList;
            }
            
            return  View();
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
