namespace WebApiDemoApp.Models
{
    public class User : IdentityUser
    {
        //[ForeignKey("AuthorFK")]
        public ICollection<Post> Posts { get; set; }

    }
}