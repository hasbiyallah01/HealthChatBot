﻿using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Models.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HealthChatBox.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IUserService _userService;
        private ILogger<UserController> _logger;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _userService.GetUser(id);
            if (!user.IsSuccessful || user == null)
            {
                _logger.LogError("User not found: {UserId}", id);
                return NotFound(new { Message = user.Message });
            }
            var result = new JsonResult(user.Value)
            {
                StatusCode = (int?)HttpStatusCode.OK
            };
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserRequest request)
        {
            var userResponse = await _userService.UpdateUser(id, request);
            if (userResponse.IsSuccessful)
            {
                _logger.LogInformation("User updated successfully: {UserId}", id);
                return Ok(new { Message = userResponse.Message });
            }
            _logger.LogError("User update failed: {UserMessage}", userResponse.Message);
            return BadRequest(new { Message = userResponse.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var userResponse = await _userService.RemoveUser(id);
            if (userResponse.IsSuccessful)
            {
                _logger.LogInformation("User deleted successfully: {UserId}", id);
                return Ok(new { Message = userResponse.Message });
            }
            _logger.LogError("User deletion failed: {UserMessage}", userResponse.Message);
            return BadRequest(new { Message = userResponse.Message });
        }
    }
}
