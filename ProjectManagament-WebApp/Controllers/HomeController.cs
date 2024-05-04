using Microsoft.AspNetCore.Mvc;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Helpers;
using ProjectManagament_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public async Task<IActionResult> GetModuleContentAsync(Guid moduleId)
        {
            // Logic to fetch and return module content based on moduleId
            // For example:
            var moduleContent = await _context.ConversationHistories.Where(c => c.ModuleId == moduleId && c.UserId == UserHelper.GetUserId(User) && c.IsDeleted == false).OrderBy(c => c.CreatedAt).ToListAsync();
            return Ok(moduleContent);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
