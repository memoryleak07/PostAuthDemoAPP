using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemoApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PostsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: api/Posts/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPost()
        {
            var posts = await _context.Posts
                .Select(x => PostDTO(x))
                .ToListAsync();

            if (posts.Count == 0)
            {
                return Ok("No elements available."); // 200 Custom message
            }

            return posts;
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return PostDTO(post);
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPost(long id, PostDTO postDTO)
        {
            if (id != postDTO.PostId)
            {
                return BadRequest();
            }
            Post? post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name.ToString());

            // Update
            post.Title = postDTO.Title;
            post.Body = postDTO.Body;
            post.AuthorId = user?.Id;
            post.Updated = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(post);
            }
            catch (DbUpdateConcurrencyException) when (!PostExists(id))
            {
                return NotFound();
            }
        }

        // POST: api/Post
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO postDTO)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name.ToString());
            // Create PostItem obj
            var post = new Post
            {
                Title = postDTO.Title,
                Body = postDTO.Body,
                AuthorId = user?.Id,
                Updated = DateTime.Now, // Automatically set Datetime
            };
            // Save to db
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            // Create HTTP success response 
            return CreatedAtAction(
                nameof(GetPost),
                new { id = post.PostId },
                PostDTO(post));
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePost(long id)
        {
            // Get the Post
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            // Delete element
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            // Return 
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> FilterPosts(string? title, string? body)
        {
            IQueryable<Post> posts = _context.Posts;
            // Look for string in title or body
            if (!string.IsNullOrEmpty(title))
            {
                posts = posts.Where(x => x.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(body))
            {
                posts = posts.Where(x => x.Body.Contains(body));
            }
            // If no one element found
            if (posts.Count() == 0)
            {
                return Ok("No elements available.");
            }

            return Ok(posts);
        }
        // GET: api/Posts/user/{username}
        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetUserPosts(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            // Get all Posts of a specific Author
            var posts = await _context.Posts
                .Where(p => p.AuthorId == user.Id)
                .Select(x => PostDTO(x))
                .ToListAsync();

            if (posts.Count == 0)
            {
                return Ok("No elements available."); // 200 Custom message
            }

            return Ok(posts);
        }

        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.PostId == id);
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


//if (user == null)
//{
//    return Unauthorized("You are not authenticated.");
//}
//// Get the user's identity
//var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
//// Get the user's Name
//var userUsername = userIdentity.FindFirst(ClaimTypes.Name)?.Value;


// Automatically set the Author 
//if (!string.IsNullOrEmpty(user))
//{
//    post.Author = user;
//}