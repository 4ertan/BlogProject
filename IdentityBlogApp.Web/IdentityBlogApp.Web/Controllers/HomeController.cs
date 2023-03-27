using IdentityBlogApp.Web.Models;
using IdentityBlogApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IdentityBlogApp.Web.Extenisons;
using Microsoft.Extensions.Hosting;

namespace IdentityBlogApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
       private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null, AppDbContext appDbContext=null)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var PostList = _appDbContext.Posts.OrderByDescending(x => x.ClickCount).Take(10).ToList();
            List<PostViewModel> PostViewModelList = new List<PostViewModel>();
            

            if (PostList != null)
            {
                foreach (var item in PostList)
                {
                    PostViewModel postViewModel = new PostViewModel
                    {
                        Id = item.Id,
                        Body = item.Body,
                        Author = item.AppUser,
                        CreatedDate = item.CreatedDate,
                        ImageUrl = item.ImageUrl,
                        Title = item.Title,
                        ClickCount = item.ClickCount,

                    };

                    PostViewModelList.Add(postViewModel);

                }

                return View(PostViewModelList);
            }
            return View();
        }
        public IActionResult PostPage()
        {
            var PostList=_appDbContext.Posts.OrderByDescending(x=>x.CreatedDate).Take(10).ToList();
            List<PostViewModel> PostViewModelList= new List<PostViewModel>();
            ViewBag.TagList=_appDbContext.Tags.ToList();

            if (PostList != null)
            {
                foreach (var item in PostList)
                {
                    PostViewModel postViewModel = new PostViewModel
                    {
                        Id = item.Id,
                        Body = item.Body,
                        Author = item.AppUser,
                        CreatedDate = item.CreatedDate,
                        ImageUrl = item.ImageUrl,
                        Title = item.Title,
                       ClickCount=item.ClickCount,
                       
                    };
                    
                    PostViewModelList.Add(postViewModel);

                }
                
                return View(PostViewModelList);
            }
            return View();

        }

        public IActionResult PostDetail(int id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Hatalı işlem");
                return RedirectToAction("PostPage");
            }
            var post = _appDbContext.Posts.Find(id);
            string PostAutorId = post.AppUserId;
            var postList = _appDbContext.Posts.ToList();
            List<PostViewModel> AuthorPosts= new List<PostViewModel>();
            foreach (var item in postList)
            {
                if (item.AppUserId==PostAutorId)
                {
                    PostViewModel postViewModel = new()
                    {
                        Id = item.Id,
                        Body = item.Body,
                        Author = item.AppUser,
                        CreatedDate = item.CreatedDate,
                        ImageUrl = item.ImageUrl,
                        Title = item.Title,
                        ClickCount = item.ClickCount,
                    };
                    AuthorPosts.Add(postViewModel);

                }
            }
            ViewBag.PostAuthor = AuthorPosts;
            PostViewModel postView = new() { Body = post.Body, Title = post.Title, Author = post.AppUser, CreatedDate = post.CreatedDate, ImageUrl = post.ImageUrl };
            long sayı= post.ClickCount;
            post.ClickCount = sayı + 1;
            _appDbContext.SaveChanges();
            return View(postView); 
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult PostList()
        {
              var post=  _appDbContext.Posts.ToList();
            if (post != null)
            {
                

            return View(post);
            }

            return View();

        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model,string? ReturnUrl=null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            ReturnUrl =ReturnUrl ?? Url.Action("Index","Home");
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
            
         var identityResult=await  _userManager.CreateAsync(new() {UserName=request.UserName,PhoneNumber=request.Phone,Email=request.Email},request.PasswordConfirm);
            //await _roleManager.a
           //await _roleManager.CreateAsync(new() { Name = "Member" });
            if (identityResult.Succeeded)
            {
                AppUser user = await _userManager.FindByEmailAsync(request.Email);
                await _userManager.AddToRoleAsync(user, "Member");
               TempData["SuccessMessage"] = "Üyelik kayit işlemi başarılı";
              return RedirectToAction(nameof(HomeController.SignUp));
            }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
           
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser=await _userManager.FindByEmailAsync(request.Email);
            if (hasUser==null)
            {
                ModelState.AddModelError(string.Empty,"Bu mail adresine sahip kullanıcı bulunamamıştır.");
                return View();
            }
            string passwordResetToken =await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink=Url.Action("ResetPassword","Home",new{userId=hasUser.Id,Token=passwordResetToken});

            TempData["success"] = "Şifre yenileme linki, email adresinize gönderilmiştir.";
            return RedirectToAction("ForgetPassword","Home");


        //https://localhost:7138
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//abmzqyftxgxfwrsq