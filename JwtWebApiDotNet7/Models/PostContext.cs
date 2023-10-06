using Microsoft.EntityFrameworkCore;

namespace WebApiDemoApp.Models
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options)
            : base(options)
        {
        }

        public DbSet<PostItem> PostItems { get; set; } = null!;
    }
}
