/*using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Models.UserModel;
using Microsoft.AspNetCore.Mvc;

namespace HealthChatBox.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _config;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IUserService userService,
            IIdentityService identityService,
            IConfiguration config,
            IVerificationCodeService verificationCodeService,
            ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _identityService = identityService;
            _config = config;
            _verificationCodeService = verificationCodeService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestModel model)
        {
            var user = await _userService.Login(model);

            if (user.IsSuccessful)
            {
                var token = _identityService.GenerateToken(_config["Jwt:Key"], _config["Jwt:Issuer"], user.Value);
                var loginResponse = new LoginResponseModel
                {
                    IsSuccessful = true,
                    Message = "Login successful.",
                    Data = user.Value,
                    Token = token
                };
                return Ok(loginResponse);
            }
            else
            {
                return BadRequest(new { Message = user.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            var user = await _userService.CreateUser(request);

            if (user.IsSuccessful)
            {
                _logger.LogInformation("User registered successfully: {UserId}", user.Value.Id);
                return Ok(new { Message = "User registered successfully. Please verify your email." });
            }
            else
            {
                _logger.LogError("User registration failed: {Message}", user.Message);
                return BadRequest(new { Message = user.Message });
            }
        }

        [HttpGet("verify-code")]
        public async Task<IActionResult> VerifyCode(int userId, int code)
        {
            var verifyCodeResponse = await _verificationCodeService.VerifyCode(userId, code);
            if (verifyCodeResponse.IsSuccessful)
            {
                _logger.LogInformation("User verified successfully: {UserId}", userId);
                return Ok(new { Message = "Email verified successfully." });
            }
            else
            {
                _logger.LogError("Verification code failed for user: {UserId}", userId);
                return BadRequest(new { Message = "Verification failed." });
            }
        }
    }
}

*/



using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Models.UserModel;
using Microsoft.AspNetCore.Mvc;

public class AuthenticationController : Controller
{
    private readonly IUserService _userService;
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _config;
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IUserService userService,
        IIdentityService identityService,
        IConfiguration config,
        IVerificationCodeService verificationCodeService,
        ILogger<AuthenticationController> logger)
    {
        _userService = userService;
        _identityService = identityService;
        _config = config;
        _verificationCodeService = verificationCodeService;
        _logger = logger;
    }

    [HttpPost("signup-login")]
    public async Task<IActionResult> SignUpLogin([FromForm] UserRequest request)
    {
        // Check if user exists
        var existingUser = await _userService.GetUser(request.Email);
        if (!existingUser.IsSuccessful)
        {
            // User does not exist, register them
            var registerResponse = await _userService.CreateUser(request);
            if (!registerResponse.IsSuccessful)
            {
                _logger.LogError("User registration failed: {Message}", registerResponse.Message);
                return BadRequest(new { Message = registerResponse.Message });
            }
            _logger.LogInformation("User registered successfully: {UserId}", registerResponse.Value.Id);
        }

        // Now login the user
        var loginModel = new LoginRequestModel
        {
            Email = request.Email,
            Password = request.Password // You might need to adjust this depending on your login logic
        };

        return await Login(loginModel);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequestModel model)
    {
        var user = await _userService.Login(model);

        if (user.IsSuccessful)
        {
            var token = _identityService.GenerateToken(_config["Jwt:Key"], _config["Jwt:Issuer"], user.Value);
            var loginResponse = new LoginResponseModel
            {
                IsSuccessful = true,
                Message = "Login successful.",
                Data = user.Value,
                Token = token
            };
            return RedirectToAction("CompleteProfile", "Profile", new { userId = user.Value.Id });
        }
        else
        {
            return BadRequest(new { Message = user.Message });
        }
    }

    [HttpGet("verify-code")]
    public async Task<IActionResult> VerifyCode(int userId, int code)
    {
        var verifyCodeResponse = await _verificationCodeService.VerifyCode(userId, code);
        if (verifyCodeResponse.IsSuccessful)
        {
            _logger.LogInformation("User verified successfully: {UserId}", userId);
            return Ok(new { Message = "Email verified successfully." });
        }
        else
        {
            _logger.LogError("Verification code failed for user: {UserId}", userId);
            return BadRequest(new { Message = "Verification failed." });
        }
    }
}
