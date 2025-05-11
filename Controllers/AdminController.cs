using BookHaven.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpDelete("delete-user/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        // Optional: Check if current user is Admin (based on authentication setup)

        var result = await _userService.DeleteUserAsync(userId);
        if (!result)
            return NotFound(new { message = "User not found." });

    //    return Ok(new { message = "User deleted successfully." });
    //}
}
