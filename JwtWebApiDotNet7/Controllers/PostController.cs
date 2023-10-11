using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDemoApp.Interfaces;

namespace WebApiDemoApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public PostController(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }
        // GET: api/Posts/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPost()
        {
            var posts = await _postService.GetAllPosts();
            if (posts.Count == 0)
            {
                return NotFound();
            }

            return Ok(posts);
        }
        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(long id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        // PUT: api/Posts/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPost(long id, PostDTO postDTO)
        {
            // Get Post by Id
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            // Get the current User
            var currentUser = await _userService.GetUserByUserName(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            // Update Post
            post.PostId = id;
            post.Title = postDTO.Title;
            post.Body = postDTO.Body;
            post.AuthorId = currentUser.Id;
            post.Updated = DateTime.Now;
            // Update database
            await _postService.UpdatePost();

            return Ok(post);
        }
        // POST: api/Post
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPost(PostDTO postDTO)
        {
            var currentUser = await _userService.GetUserByUserName(HttpContext.User.Identity.Name);
            // Create PostItem obj
            var post = new Post
            {
                Title = postDTO.Title,
                Body = postDTO.Body,
                AuthorId = currentUser?.Id,
                Updated = DateTime.Now, // Automatically set Datetime
            };
            // Save to db
            await _postService.CreatePost(post);
     
            return Ok(post);
        }
        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePost(long id)
        {
            // Get the Post
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            // Delete element by PK Id
            await _postService.DeletePostById(id);

            return Ok();
        }
        // GET: api/Post/search?title=Title&body=Body
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> FilterPosts(string? title, string? body)
        {
            var posts = await _postService.GetAllPosts();

            List<Post> filteredPosts = await _postService.FilterPosts(title, body, posts);

            // If no one element found
            if (filteredPosts.Count == 0)
            {
                return Ok("No elements available.");
            }

            return Ok(filteredPosts);
        }
        // GET: api/Posts/user/{username}
        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetUserPosts(string username)
        {
            var user = await _userService.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }
            // Get all Posts of a specific Author
            var posts = await _postService.GetPostsByAuthorId(user.Id);

            if (posts.Count == 0)
            {
                return Ok("No elements available."); // 200 Custom message
            }

            return Ok(posts);
        }
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



//private async Task<User?> GetUser()
//{
//    User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
//    return user;
//}

//private bool PostExists(long id)
//{
//    return _context.Posts.Any(e => e.PostId == id);
//}

//private static PostDTO PostDTO(Post post) =>
//   new()
//   {
//       PostId = post.PostId,
//       Title = post.Title,
//       Body = post.Body,
//       AuthorId = post.AuthorId,
//       Updated = post.Updated,
//   };