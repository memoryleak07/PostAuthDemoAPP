namespace WebApiDemoApp.Models
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }
    }
}
