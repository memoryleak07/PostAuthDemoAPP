using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthController(IConfiguration configuration, ApplicationDbContext context, UserManager<User> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }
        // Test method
        [HttpGet]
        [Route("TestAuth")]
        [Authorize(Roles = "User", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> TestAuthorizationAsync()
        {
            return Ok("You're Authorized");
        }
        // Signup
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterAsync(UserDTO request)
        {
            // Check if Username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Assign Role
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                // Handle role assignment failure, if needed
                return BadRequest("Failed to assign role to user.");
            }
            return Ok(user);
        }
        // Login
        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO request)
        {
            // Find the user
            var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
            {
                return BadRequest("Wrong credentials.");
            }

            string token = CreateToken(user, "User");

            return Ok(token);
        }
        // Create Token
        private string CreateToken(User user, string role)
        {
            // Create user with role User
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role),
            };
            // Generate token from the app token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(15),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
