using Microsoft.AspNetCore.Mvc;
using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Options;
using Microsoft.Extensions.Options;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtOptions _jwtOptions;

        public UserController(IUserService userService, IOptions<JwtOptions> jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            request.Name = request.Name.Replace(" ", "");

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
                UserName = request.Name, // Required by Identity
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Validate the user
            var user = await _userService.LoginAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            // Generate the JWT token for the logged-in user
            var token = _userService.GenerateJwtToken(user);

            // Return the response with the token
            var response = new UserResponse
            {
                UserID = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token // Attach the JWT token to the response
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
        // PUT: api/user/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, request);
            if (updatedUser == null)
                return NotFound("User not found.");

            var response = new UserResponse
            {
                UserID = updatedUser.UserID,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                Role = updatedUser.Role
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
