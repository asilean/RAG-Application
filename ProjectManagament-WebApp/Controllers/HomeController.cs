using Microsoft.AspNetCore.Mvc;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Data.Models;
using ProjectManagament_WebApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjectManagament_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PMContext _context;
        public HomeController(ILogger<HomeController> logger, PMContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        //public IActionResult GetModuleContent(Guid moduleId)
        //{
        //    // Logic to fetch and return module content based on moduleId
        //    // For example:
        //    var moduleContent = _context.GetModuleContentById(moduleId);
        //    return PartialView("_ModuleContent", moduleContent);
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
