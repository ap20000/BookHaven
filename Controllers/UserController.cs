using Microsoft.AspNetCore.Mvc;
using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Services;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!request.Email.Contains("@"))
                return BadRequest("Invalid email format.");

            if (request.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            var existing = await _userService.FindByEmailAsync(request.Email);
            if (existing != null)
                return BadRequest("User with this email already exists.");

            var newUser = new ApplicationUser
            {
                FullName = request.Name,
                Email = request.Email,
                UserName = request.Email, // Required by Identity
                Role = request.Role,
                MembershipId = Guid.NewGuid().ToString(),
                SuccessfulOrderCount = 0
            };

            var createdUser = await _userService.RegisterAsync(newUser, request.Password);

            var response = new UserResponse
            {
                UserID = createdUser.Id,
                Name = createdUser.FullName,
                Email = createdUser.Email,
                Role = createdUser.Role
            };

            return Ok(response);
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.LoginAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var response = new UserResponse
            {
                UserID = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(response);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var response = new UserResponse
            {
                UserID = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(response);
        }

        // GET: api/user/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var responses = users.Select(u => new UserResponse
            {
                UserID = u.Id,
                Name = u.FullName,
                Email = u.Email,
                Role = u.Role
            });

            return Ok(responses);
        }

        // GET: api/user/role/{role}
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var users = await _userService.GetAllByRoleAsync(role);

            var responses = users.Select(u => new UserResponse
            {
                UserID = u.Id,
                Name = u.FullName,
                Email = u.Email,
                Role = u.Role
            });

            return Ok(responses);
        }
    }
}
