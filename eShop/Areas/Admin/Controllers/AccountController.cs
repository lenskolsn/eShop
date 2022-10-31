using eShop.Areas.Admin.ViewModels.Account;
using eShop.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.Areas.Admin.Controllers
{
    [Area("Admin")] // bắt buộc đăng nhập
    public class AccountController : BaseController
    {
        public AccountController(AppDbContext db) : base(db)
        {
        }
        [Route("/login")]
        [Route("/{area}/{controler}/{action}")]
        [AllowAnonymous] // cho phép truy cập ko cần đăng nhập
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("/login")]
        [Route("/{area}/{controler}/{action}")]
        [AllowAnonymous]
        public IActionResult Login(LoginVM model)
        {
            var data = _db.Users.SingleOrDefault(x => x.Username == model.Username);
            if (data == null)
            {
                TempData["error"] = "Tên đăng nhập không hợp lệ";
                return View(model);
            }
            if(model.Password != data.Password)
            {
                TempData["error"] = "Mật khẩu không hợp lệ";
                return View(model);
            }
            // Lưu thông tin hiện tại của User
            var claims = new List<Claim> {
                            new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                            new Claim("Username",data.Username),
                            new Claim("Fullname",data.Fullname),
                            new Claim(ClaimTypes.Role, data.Role?.ToString()),

                        };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(claimsIdentity);
            var authenPropeties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.UtcNow.AddHours(10),
                IsPersistent = model.RememberMe
            };
            HttpContext.SignInAsync("Cookies", principal, authenPropeties).Wait();

            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Redirect("/login");
        }
    }
}
