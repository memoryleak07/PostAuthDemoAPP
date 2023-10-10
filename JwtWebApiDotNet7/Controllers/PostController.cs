using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDemoApp.Interfaces;

namespace WebApiDemoApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;


        public PostController(ApplicationDbContext context, UserManager<User> userManager, IPostService postService)
        {
            _context = context;
            _userManager = userManager;
            _postService = postService;
        }
        // GET: api/Posts/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPost()
        {
            //var posts1 = await _context.Posts
            //    .Select(x => PostDTO(x))
            //    .ToListAsync();
            var posts = await _postService.GetAllPosts();
            if (posts.Count() == 0)
            {
                return NotFound();
            }
            return Ok(posts);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(long id)
        {
            //var post = await _context.Posts.FindAsync(id);
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPost(long id, PostDTO postDTO)
        {
            if (id != postDTO.PostId)
            {
                return BadRequest();
            }

            User? user = await GetUser();

            // IPostService to Update the post
            if (await _postService.UpdatePostById(id, postDTO, user?.Id))
            {
                return Ok(); // Successfully updated
            }
            else
            {
                return NotFound(); // Post not found or failed to update
            }
        }
            //var post = await _postService.GetPostById(id);
            //if (post == null)
            //{
            //    return NotFound();
            //}

            //User? user = await GetUser();

            //// Update
            //post.Title = postDTO.Title;
            //post.Body = postDTO.Body;
            //post.AuthorId = user?.Id;
            //post.Updated = DateTime.Now;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //    return Ok(post);
            //}
            //catch (DbUpdateConcurrencyException) when (!PostExists(id))
            //{
            //    return NotFound();
            //}

        // POST: api/Post
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO postDTO)
        {
            User? user = await GetUser();
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
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePost(long id)
        {
            // Get the Post
            PostDTO? post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            // Delete element
            //_context.Posts.Remove(post);
            //await _context.SaveChangesAsync();
            //await _postService.DeletePost(post);
            await _postService.DeletePostById(id);
            // Return 
            return Ok();
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

        private async Task<User>? GetUser()
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            return user;
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


//Test Role
//[HttpGet("TestRole")]
//[Authorize(AuthenticationSchemes = "Bearer")]
//public async Task<ActionResult<IEnumerable<PostDTO>>> GetRole()
//{
//    // Get the current user
//    User? user = await GetUser();

//    if (user == null)
//    {
//        return NotFound("User not found.");
//    }

//    // Check if the user is in the "Admin" role (or any other role you want to check)
//    bool isInAdminRole = await _userManager.IsInRoleAsync(user, "Admin");

//    if (isInAdminRole)
//    {
//        return Ok("User is in the 'Admin' role.");
//    }
//    else
//    {
//        return Ok("User is not in the 'Admin' role.");
//    }
//}