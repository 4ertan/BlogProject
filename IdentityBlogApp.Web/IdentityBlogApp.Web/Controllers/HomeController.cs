using IdentityBlogApp.Web.Models;
using IdentityBlogApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IdentityBlogApp.Web.Extenisons;

namespace IdentityBlogApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model,string? ReturnUrl=null)
        {
            ReturnUrl=ReturnUrl ?? Url.Action("Index","Home");
            var hasUser=await _userManager.FindByEmailAsync(model.Email);

            if (hasUser==null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return View();
            }
            
            var SignInResult =await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe,true);
            if (SignInResult.Succeeded)
            {
                return Redirect(ReturnUrl);
            }

            if (SignInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "Lütfen daha sonra tekrar deneyiniz" });
                return View();
            }
            ModelState.AddModelErrorList(new List<string>() { "Email veya Şifre yanlış" });
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
        IdentityResult identityResult=await  _userManager.CreateAsync(new() {UserName=request.UserName,PhoneNumber=request.Phone,Email=request.Email},request.PasswordConfirm);

            if (identityResult.Succeeded)
            {
               TempData["SuccessMessage"] = "Üyelik kayit işlemi başarılı";
              return RedirectToAction(nameof(HomeController.SignUp));
            }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
           
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}