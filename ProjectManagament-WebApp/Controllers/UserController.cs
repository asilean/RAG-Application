using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Data.Models;
using ProjectManagament_WebApp.Models;
using System.Security.Claims;

namespace ProjectManagament_WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly PMContext _context;

        public UserController(PMContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginViewModel.Email && u.Password == loginViewModel.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password";

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SendCode(string email)
        {
            return Json(new { success = true });
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == viewModel.Email);
            ViewBag.RenewPasswordUserId = user.Id;

            return View("RenewPassword", new { userId = user.Id });
        }

        public IActionResult RenewPassword(Guid userId)
        {
            RenewPasswordViewModel viewModel = new RenewPasswordViewModel();
            viewModel.UserId = userId;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RenewPassword(RenewPasswordViewModel viewModel)
        {
            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password must be the same";
                return View(viewModel);
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == viewModel.UserId);
            user.Password = viewModel.Password;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
