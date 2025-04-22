public interface IUserService
{
    Task<User> RegisterAsync(User user);
    Task<User?> LoginAsync(string email, string password);
    Task<List<Member>> GetAllMembersAsync();
    Task<List<Admin>> GetAllAdminsAsync();
}
