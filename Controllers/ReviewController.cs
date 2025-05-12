using BookHaven.DTOs;
using BookHaven.Services;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitReview([FromBody] ReviewDto reviewDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reviewService.SubmitReviewAsync(userId, reviewDto);
            if (!result)
                return BadRequest("You can only review books you have purchased.");
            return Ok("Review submitted successfully.");
        }
    }
}
