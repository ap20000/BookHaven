using BookHaven.DTOs;

public interface IUserService
{
    Task<User> RegisterAsync(User user);
    Task<User?> LoginAsync(string email, string password);
    Task<List<Member>> GetAllMembersAsync();
    Task<List<Admin>> GetAllAdminsAsync();
    Task<User?> FindByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<bool> DeleteUserAsync(int userId);

    Task<User?> UpdateUserAsync(int userId, UpdateProfileRequest request);

}
