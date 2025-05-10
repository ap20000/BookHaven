using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookHaven.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _jwtOptions;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
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
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            return usersInRole.ToList();
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwtOptions.ExpiryHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public async Task<ApplicationUser?> UpdateUserAsync(string userId, UpdateProfileRequest request)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null) return null;

        //    user.UserName = request.Name ?? user.UserName;
        //    user.Email = request.Email ?? user.Email;

        //    if (!string.IsNullOrEmpty(request.Password))
        //    {
        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
        //        if (!passwordResult.Succeeded)
        //        {
        //            throw new Exception(string.Join("; ", passwordResult.Errors.Select(e => e.Description)));
        //        }
        //    }

        //    var updateResult = await _userManager.UpdateAsync(user);
        //    return updateResult.Succeeded ? user : null;
        //}

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
