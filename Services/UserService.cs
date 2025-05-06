using BookHaven.DTOs;
using BookHaven.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<List<Admin>> GetAllAdminsAsync()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> UpdateUserAsync(int userId, UpdateProfileRequest request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.Name = request.Name;
            user.Password = request.Password;

            if (user is Member member && request.Phone != null)
            {
                member.Phone = request.Phone;
            }

            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
