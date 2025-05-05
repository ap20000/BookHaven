using BookHaven.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return user;
        }

        public async Task<ApplicationUser?> LoginAsync(string email, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                    return user;
            }
            return null;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetAllByRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role) as List<ApplicationUser>;
        }
    }
}
