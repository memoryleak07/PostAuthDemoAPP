using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemoApp.Models;

namespace WebApiDemoApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostItemsController : ControllerBase
    {
        private readonly PostContext _context;

        public PostItemsController(PostContext context)
        {
            _context = context;
        }

        // GET: api/PostItems/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPostItems()
        {
            var postItems = await _context.PostItems
                .Select(x => ItemPostDTO(x))
                .ToListAsync();

            if (postItems.Count == 0)
            {
                return Ok("No elements available."); // 200 Custom message
            }

            return postItems;
        }

        // GET: api/PostItems/5
        // <snippet_GetByID>
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPostItem(long id)
        {
            var postItem = await _context.PostItems.FindAsync(id);

            if (postItem == null)
            {
                return NotFound();
            }

            return ItemPostDTO(postItem);
        }
        // </snippet_GetByID>

        // PUT: api/PostItems/5
        // <snippet_Update>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPostItem(long id, PostDTO postDTO)
        {
            if (id != postDTO.Id)
            {
                return BadRequest();
            }
            var postItem = await _context.PostItems.FindAsync(id);
            if (postItem == null)
            {
                return NotFound();
            }
            // Check if Owner
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userUsername = userIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (postItem.Author != userUsername)
            {
                return Forbid(); // Or return a 403 Forbidden response
            }
            // Finally try PUT changes
            postItem.Title = postDTO.Title;
            postItem.Body = postDTO.Body;
            postItem.Author = userUsername;
            postItem.Updated = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(postItem);
            }
            catch (DbUpdateConcurrencyException) when (!PostItemExists(id))
            {
                return NotFound();
            }
        }
        // </snippet_Update>

        // POST: api/PostItems
        // <snippet_Create>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostDTO>> PostTodoItem(PostDTO postDTO)
        {
            // Get the user's identity
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            // Get the user's Name
            var userUsername = userIdentity.FindFirst(ClaimTypes.Name)?.Value;
            // Create PostItem obj
            var postItem = new PostItem
            {
                Title = postDTO.Title,
                Body = postDTO.Body,
                Updated = DateTime.Now, // Automatically set Datetime
            };
            // Automatically set the Author 
            if (!string.IsNullOrEmpty(userUsername))
            {
                postItem.Author = userUsername;
            }

            _context.PostItems.Add(postItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPostItem),
                new { id = postItem.Id },
                ItemPostDTO(postItem));
        }
        // </snippet_Create>

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePostItem(long id)
        {
            // Get the Post
            var postItem = await _context.PostItems.FindAsync(id);
            if (postItem == null)
            {
                return NotFound();
            }
            // If Owner
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userUsername = userIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (postItem.Author != userUsername)
            {
                return Forbid(); // Or return a 403 Forbidden response
            }
            // Delete
            _context.PostItems.Remove(postItem);
            await _context.SaveChangesAsync();
            // Return 
            return NoContent();
        }

        private bool PostItemExists(long id)
        {
            return _context.PostItems.Any(e => e.Id == id);
        }

        private static PostDTO ItemPostDTO(PostItem postItem) =>
           new PostDTO
           {
               Id = postItem.Id,
               Title = postItem.Title,
               Body = postItem.Body,
               Author = postItem.Author,
               Updated = postItem.Updated,
           };
    }
}
