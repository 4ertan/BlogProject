using IdentityBlogApp.Web.Areas.Admin.Models;
using IdentityBlogApp.Web.Extenisons;
using IdentityBlogApp.Web.Models;
using IdentityBlogApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace IdentityBlogApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext appDbContext, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _fileProvider = fileProvider;
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
                Bio = user.Bio


            };

            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
            return View();

            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            currentUser.UserName= model.UserName;
            currentUser.Email= model.Email;
            currentUser.PhoneNumber = model.Phone;
            currentUser.Gender= model.Gender;
            currentUser.BirthDate= model.BirthDate;
            currentUser.Bio=model.Bio;
            

            if (model.Picture != null && model.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.Picture.FileName)}";
                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath, randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await model.Picture.CopyToAsync(stream);
                //model.Picture = randomFileName;
            }
            TempData["Success"] = "Güncelleme Başarılı";
            await _userManager.UpdateAsync(currentUser);
            return RedirectToAction("UserEdit");

        }
        public IActionResult PostAdd()
        {
            var taglist = new List<SelectListItem>();
            var tags=_appDbContext.Tags.ToList();

            foreach (var item in tags)
            {
                taglist.Add(new SelectListItem
                {
                    Text=item.Name,
                    Value=item.Id.ToString(),
                });
            }
            ViewBag.taglist=taglist;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostAdd(PostViewModel model,List<string> tagList)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Hatalı işlem");
            return  View();
            }
            
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            Post post = new Post();
            post.Title=model.Title;
            post.Body=model.Body;
            post.AppUser=currentUser;

            foreach (string item in tagList)
            {
                post.PostTags.Add(new() {Name=item});
            }
            post.ImageUrl=model.ImageUrl;
            _appDbContext.Add(post);
            _appDbContext.SaveChanges();
            TempData["Success"] = "Post başarılı";
            return RedirectToAction("PostAdd");
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
