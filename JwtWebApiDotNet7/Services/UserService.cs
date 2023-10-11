using WebApiDemoApp.Interfaces;

namespace WebApiDemoApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User>? GetUserByUserName(string userName)
        {
            var user = await _context.Users.FindAsync(u => u.UserName == userName);
            return user;
        }


        public async Task<bool> AssignRoleToUser(User user, string roleName)
        {
            // Assign Role
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if(!roleResult.Succeeded)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
