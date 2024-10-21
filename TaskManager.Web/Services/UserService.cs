using Microsoft.AspNetCore.Identity;
namespace TaskManager.Web.Services
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string email, string password);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }

    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }



    namespace TaskManager.Web.Services
    {
        public class UserService : IUserService
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;

            public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<User> ValidateUserAsync(string email, string password)
            {
                var identityUser = await _userManager.FindByEmailAsync(email);
                if (identityUser == null)
                {
                    return null;
                }

                var result = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
                if (result.Succeeded)
                {
                    return new User
                    {
                        Id = identityUser.Id,
                        UserName = identityUser.UserName,
                        Email = identityUser.Email
                    };
                }

                return null;
            }

            public async Task<IList<string>> GetUserRolesAsync(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new List<string>();
                }

                return await _userManager.GetRolesAsync(user);
            }
        }
    }
}