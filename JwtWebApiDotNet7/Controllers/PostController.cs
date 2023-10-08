using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemoApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PostController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Test method
        [Authorize(Roles = "User", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("api/TestAuth")]
        public async Task<IActionResult> TestAuthorizationAsync()
        {
            return Ok("You're Authorized");
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

        // PUT
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

        // POST: api/Posts
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

        // DELETE
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