namespace WebApiDemoApp.Interfaces
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<User?> GetUserByUserName(string userName);
        Task<bool> AssignRoleToUser(User user, string roleName);
    }
}
