namespace WebApiDemoApp.Models
{
    public class User : IdentityUser
    {
        public ICollection<Post> Posts { get; set; }

    }
}