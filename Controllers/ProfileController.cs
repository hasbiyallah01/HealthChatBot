/*using System.Threading.Tasks;
using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Models.ProfileModel;
using Microsoft.AspNetCore.Mvc;

namespace HealthChatBox.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CompleteProfile()
        {
            ViewBag.UserId = "some-user-id";
            return View(new ProfileRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(ProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _profileService.CreateProfile(request.UserId, request);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(request);
            }

            return RedirectToAction("Chatbox");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var response = await _profileService.GetAllProfiles();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var response = await _profileService.GetProfile(id);
            if (!response.IsSuccessful || response == null)
            {
                return NotFound(response?.Message);
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] ProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _profileService.UpdateProfile(id, request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProfile(int id)
        {
            var response = await _profileService.RemoveProfile(id);
            if (!response.IsSuccessful)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("Chatbox")]
        public IActionResult Chatbox()
        {
            return View();
        }
    }
}


*/


using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Models.ProfileModel;
using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public IActionResult CompleteProfile(int userId)
    {
        ViewBag.UserId = userId;
        return View(new ProfileRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteProfile(ProfileRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var response = await _profileService.CreateProfile(request.UserId, request);
        if (!response.IsSuccessful)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(request);
        }

        return RedirectToAction("Chatbox", "Profile");
    }

    [HttpGet("Chatbox")]
    public IActionResult Chatbox()
    {
        return View();
    }
}
