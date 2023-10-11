namespace WebApiDemoApp.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPosts();
        Task<Post>? GetPostById(long id);
        Task<List<Post>>? GetPostsByAuthorId(string name);
        Task CreatePost(Post post);
        Task UpdatePost();
        Task DeletePostById(long id);
        Task<List<Post>> FilterPosts(string? title, string? body, List<Post> posts);
        bool PostIdExists(long id);
    }
}
