using Microsoft.AspNetCore.Mvc;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Helpers;
using ProjectManagament_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ProjectManagament_WebApp.Data.Models;

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

        [HttpPost]
        public async Task<IActionResult> ConversationAsync([FromBody] Conversation conversation)
        {
            // Logic to save module content based on moduleId
            var questionHistory = new ConversationHistory
            {
                UserId = (Guid)UserHelper.GetUserId(User),
                ModuleId = conversation.ModuleId,
                Context = conversation.Question,
                Role = Role.User,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.ConversationHistories.Add(questionHistory);
            await _context.SaveChangesAsync();

            // Make request to bot service to get response
            // For example: _botService.GetBotResponse(conversation.Question, conversation.ModuleId);
            var response = "Bot response";

            // Save bot response to database
            var responseHistory = new ConversationHistory
            {
                UserId = (Guid)UserHelper.GetUserId(User),
                ModuleId = conversation.ModuleId,
                Context = response,
                Role = Role.Chatbot,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.ConversationHistories.Add(responseHistory);
            await _context.SaveChangesAsync();

            return Ok(new { answer = response });
        }

        [HttpGet]
        public IActionResult DeleteChat(Guid moduleId)
        {
            _context.ConversationHistories.Where(c => c.ModuleId == moduleId && c.UserId == UserHelper.GetUserId(User)).ToList().ForEach(c => c.IsDeleted = true);
            _context.SaveChanges();
            
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
