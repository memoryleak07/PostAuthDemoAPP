using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDemoApp.Interfaces;
using WebApiDemoApp.Services;

namespace WebApiDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // Test method
        [HttpGet]
        [Route("TestAuth")]
        [Authorize(Roles = "User", AuthenticationSchemes = "Bearer")]
        public Task<IActionResult> TestAuthorization()
        {
            return Task.FromResult<IActionResult>(Ok("You're Authorized"));
        }
        // Signup
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterAsync(UserDTO request)
        {
            // Check if Username already exists
            var existingUser = await _userService.GetUserByUserName(request.UserName);

            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }
            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash)
            };

            // Add the user to Db
            await _userService.AddUser(user);
            // Assign Role
            await _userService.AssignRoleToUser(user, "User");

            return Ok(user);
        }
        // Login
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(UserDTO request)
        {
            // Find the user
            User? user = await _userService.GetUserByUserName(request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
            {
                return BadRequest("Wrong credentials.");
            }
            // Generate JWT token
            AuthService authService = new();

            string token = authService.CreateToken(user.UserName, "User");

            return Ok(token);
        }
    }
}
