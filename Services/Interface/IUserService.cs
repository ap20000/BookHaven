using BookHaven.DTOs;
using BookHaven.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserService
{
    Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password);
    Task<ApplicationUser?> LoginAsync(string email, string password);
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<List<ApplicationUser>> GetAllByRoleAsync(string role);
    Task<ApplicationUser?> UpdateUserAsync(string userId, UpdateProfileRequest request);
    Task<bool> DeleteUserAsync(string userId);
    string GenerateJwtToken(ApplicationUser user);
}
