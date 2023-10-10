using Microsoft.AspNetCore.Mvc;
using WebApiDemoApp.Interfaces;

namespace WebApiDemoApp.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context) 
        { 
            _context = context; 
        }
        public async Task<IEnumerable<PostDTO>>? GetAllPosts()
        {
            var posts = await _context.Posts
                .Select(x => PostDTO(x))
                .ToListAsync();

            return posts;
        }
        public async Task<PostDTO> GetPostById(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            return PostDTO(post);
        }
        public async void DeletePost(Post post)
        {
            // Delete element
            _context.Posts.Remove(post); 
            await _context.SaveChangesAsync();
        }




        // Delete element
        //_context.Posts.Remove(post);
        //await _context.SaveChangesAsync();

        private static PostDTO PostDTO(Post post) =>
        new()
        {
            PostId = post.PostId,
            Title = post.Title,
            Body = post.Body,
            AuthorId = post.AuthorId,
            Updated = post.Updated,
        };

    }
    
    
}
