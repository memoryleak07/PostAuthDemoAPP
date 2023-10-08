using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiDemoApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)            // Post has one Author
                .WithMany(u => u.Posts)           // User can have many Posts
                .HasForeignKey(p => p.AuthorId);  // Foreign key property
            // Adding Roles
            SeedRoles(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole()
                {
                    Name = "User",
                    ConcurrencyStamp = "2",
                    NormalizedName = "USER",
                },
                new IdentityRole()
                {
                    Name = "Member",
                    ConcurrencyStamp = "3",
                    NormalizedName = "MEMBER"
                }
            );
        }
    }
}


