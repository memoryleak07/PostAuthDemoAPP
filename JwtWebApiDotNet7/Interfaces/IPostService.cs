using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiDemoApp.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO>> GetAllPosts();
        Task<PostDTO> GetPostById(long id);
        Task<IActionResult> DeletePost(PostDTO post);

    }
}
