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
        public async Task<List<Post>> GetAllPosts()
        {
            return await _context.Posts.ToListAsync();
        }
        public async Task<Post>? GetPostById(long id)
        {
            return await _context.Posts.FindAsync(id);
        }
        public async Task<List<Post>>? GetPostsByAuthorId(string authorId)
        {
            return await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .ToListAsync();
        }
        public async Task DeletePostById(long id)
        {
            // Delete element
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        public async Task CreatePost(Post post)
        {
            // Add element
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePost()
        {
            // Update db
            await _context.SaveChangesAsync();
        }
        public bool PostIdExists(long id)
        {
            return _context.Posts.Any(p => p.PostId == id);
        }
        public async Task<List<Post>> FilterPosts(string? title, string? body, List<Post> posts)
        {
            if (!string.IsNullOrEmpty(title))
            {
                posts = posts.Where(x => x.Title.Contains(title)).ToList();
            }

            if (!string.IsNullOrEmpty(body))
            {
                posts = posts.Where(x => x.Body.Contains(body)).ToList();
            }

            return posts;
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
        //public async Task<bool> UpdatePostById(long id, PostDTO postDTO, string userId)
        //{
        //    var post = await _context.Posts.FindAsync(id);
        //    if (post == null)
        //    {
        //        return false;
        //    }

        //    // Update the post entity
        //    post.Title = postDTO.Title;
        //    post.Body = postDTO.Body;
        //    post.AuthorId = userId;
        //    post.Updated = DateTime.Now;

        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //private static PostDTO PostDTO(Post post) =>
        //new()
        //{
        //    PostId = post.PostId,
        //    Title = post.Title,
        //    Body = post.Body,
        //    AuthorId = post.AuthorId,
        //    Updated = post.Updated,
        //};

    }
    
    
}
