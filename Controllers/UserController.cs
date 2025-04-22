using Microsoft.AspNetCore.Mvc;
using BookHaven.Entities;
using BookHaven.Services.Interfaces;
using BookHaven.DTOs;

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

            User newUser;
            if (request.Role == "Member")
            {
                newUser = new Member
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    Role = "Member",
                    Phone = request.Phone,
                    MembershipID = Guid.NewGuid().ToString(),
                    TotalOrdersCount = 0
                };
            }
            else if (request.Role == "Admin")
            {
                newUser = new Admin
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    Role = "Admin"
                };
            }
            else
            {
                return BadRequest("Invalid role. Must be 'Member' or 'Admin'.");
            }

            var createdUser = await _userService.RegisterAsync(newUser);

            var response = new UserResponse
            {
                UserID = createdUser.UserID,
                Name = createdUser.Name,
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
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(response);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var response = new UserResponse
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(response);
        }

        // GET: api/user/members
        [HttpGet("members")]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _userService.GetAllMembersAsync();

            var responses = members.Select(m => new UserResponse
            {
                UserID = m.UserID,
                Name = m.Name,
                Email = m.Email,
                Role = m.Role
            });

            return Ok(responses);
        }

        // GET: api/user/admins
        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _userService.GetAllAdminsAsync();

            var responses = admins.Select(a => new UserResponse
            {
                UserID = a.UserID,
                Name = a.Name,
                Email = a.Email,
                Role = a.Role
            });

            return Ok(responses);
        }
    }
}
