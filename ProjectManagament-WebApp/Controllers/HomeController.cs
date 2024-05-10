using Microsoft.AspNetCore.Mvc;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Helpers;
using ProjectManagament_WebApp.Models;
using ProjectManagament_WebApp.Sevices;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ProjectManagament_WebApp.Data.Models;

namespace ProjectManagament_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PMContext _context;
        private readonly ChatGptService _chatGptService;
        public HomeController(ILogger<HomeController> logger, PMContext context, ChatGptService chatGptService)
        {
            _logger = logger;
            _context = context;
            _chatGptService = chatGptService;
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
            var userId = UserHelper.GetUserId(User);
            if (!userId.HasValue)
            {
                return BadRequest("User ID is required.");
            }

            // Save user question to database
            var questionHistory = new ConversationHistory
            {
                UserId = userId.Value,
                ModuleId = conversation.ModuleId,
                Context = conversation.Question,
                Role = Role.User,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.ConversationHistories.Add(questionHistory);
            await _context.SaveChangesAsync();

            // Get response from GPT-4 based Chat Service
            var response = await _chatGptService.GetChatCompletionAsync(conversation.Question, conversation.ModuleId);

            // Save bot response to database
            var responseHistory = new ConversationHistory
            {
                UserId = userId.Value,
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
            var userId = UserHelper.GetUserId(User);
            if (!userId.HasValue)
            {
                return BadRequest("User ID is required.");
            }

            _context.ConversationHistories.Where(c => c.ModuleId == moduleId && c.UserId == userId.Value).ToList().ForEach(c => c.IsDeleted = true);
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
