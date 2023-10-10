using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiDemoApp.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        public string GetTokenKey()
        {
            var tokenKey = _configuration["AppSettings:Token"];
            return tokenKey;
        }
        // Create Token
        public string CreateToken(string userName, string roleName)
        {
            // Create user with role User
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, roleName),
            };
            // Generate token from the app token
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            //    _configuration.GetSection("AppSettings:Token").Value!));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenKey()));
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
