using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiDemoApp.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO>> GetAllPosts();
        Task<PostDTO> GetPostById(long id);
        Task DeletePostById(long id);
        //Task<bool> UpdatePostAsync(long id, PostDTO postDTO, User? user);
        Task<bool> UpdatePostById(long id, PostDTO postDTO, string userId);
    }
}
