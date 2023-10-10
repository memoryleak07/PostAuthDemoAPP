using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDemoApp.Services;

namespace WebApiDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();
        //private readonly ApplicationDbContext _context;
        //private readonly IConfiguration _configuration;
        //private readonly UserManager<User> _userManager;
        private readonly UserService _userService;


        public AuthController(/*IConfiguration configuration, ApplicationDbContext context, UserManager<User> userManager, */UserService userService)
        {
            //_configuration = configuration;
            //_context = context;
            //_userManager = userManager;
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
            //var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            User? existingUser = await _userService.GetUserByUserName(request.UserName);

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
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            await _userService.AddUser(user);
            // Assign Role
            //var roleResult = await _userManager.AddToRoleAsync(user, "User");
            //if (!roleResult.Succeeded)
            //{
            //    // Handle role assignment failure, if needed
            //    return BadRequest("Failed to assign role to user.");
            //}
            await _userService.AssignRoleToUser(user, "User");
            return Ok(user);
        }
        // Login
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(UserDTO request)
        {
            // Find the user
            //var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);
            User? user = await _userService.GetUserByUserName(request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
            {
                return BadRequest("Wrong credentials.");
            }

            AuthService authService = new();

            string token = authService.CreateToken(user.UserName, "User");

            return Ok(token);
        }
    }
}
