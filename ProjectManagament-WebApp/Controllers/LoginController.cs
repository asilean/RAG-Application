using Microsoft.AspNetCore.Mvc;

namespace ProjectManagament_WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
