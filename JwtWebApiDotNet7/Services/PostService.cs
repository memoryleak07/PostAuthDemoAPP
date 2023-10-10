using Microsoft.AspNetCore.Mvc;
using WebApiDemoApp.Interfaces;
using WebApiDemoApp.Models;

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
            return await _context.Posts
                .Select(x => PostDTO(x))
                .ToListAsync();
        }
        /// 
        /// 
        public async Task<PostDTO> GetPostById(long id)
        {
            var post = await _context.Posts.FindAsync(id);
            return PostDTO(post);
        }
        public async Task DeletePostById(long id)
        {
            // Delete element
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        //public async Task<bool> UpdatePostAsync(long id, PostDTO postDTO, User? user)
        //{
        //    var post = await _context.Posts.FindAsync(id);
        //    if (post == null)
        //    {
        //        return false;
        //    }

        //    // Update the post entity
        //    post.Title = postDTO.Title;
        //    post.Body = postDTO.Body;
        //    post.AuthorId = user?.Id;
        //    post.Updated = DateTime.Now;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // Handle concurrency conflict if necessary
        //        return false;
        //    }
        //}
        public async Task<bool> UpdatePostById(long id, PostDTO postDTO, string userId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }

            // Update the post entity
            post.Title = postDTO.Title;
            post.Body = postDTO.Body;
            post.AuthorId = userId;
            post.Updated = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

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
