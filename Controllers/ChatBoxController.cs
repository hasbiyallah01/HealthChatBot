using Microsoft.AspNetCore.Mvc;
using HealthChatBox.Models;
using OpenAI_API;
using OpenAI_API.Completions;
using HealthChatBox.Core.Application.Interfaces.Services;

namespace HealthChatBox.Controllers
{
    public class ChatBoxController : Controller
    {
        private readonly IChatService _chatService;

        public ChatBoxController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("UseChatGpt")]
        public async Task<IActionResult> UseChatGpt(string query)
        {
            string output = "";
            var openAPI = new OpenAIAPI("sk-proj-BKJ7nJ9lf4RyYMga4dyBT3BlbkFJ28dQg0WoBWKdcUXwmhfo");
            CompletionRequest request = new CompletionRequest
            {
                Prompt = query,
                Model = OpenAI_API.Models.Model.DavinciText
            };
            var completion = await openAPI.Completions.CreateCompletionAsync(request);
            foreach (var requests in completion.Completions)
            {
                output += requests.Text;
            }

            var chatEntryDto = new ChatEntryDto
            {
                Query = query,
                Response = output
            };

            await _chatService.SaveChatEntryAsync(chatEntryDto);

            return Ok(chatEntryDto);
        }
    }
}



