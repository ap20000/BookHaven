using BookHaven.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IUserService
{
    Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password);
    Task<ApplicationUser?> LoginAsync(string email, string password);
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<List<ApplicationUser>> GetAllByRoleAsync(string role);

    string GenerateJwtToken(ApplicationUser user); 
}
