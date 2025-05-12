using BookHaven.Dtos;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("{userId}/{bookId}")]
        public IActionResult AddReview(string userId, int bookId, [FromBody] ReviewDto dto)
        {
            try
            {
                _reviewService.AddReview(userId, bookId, dto.Rating, dto.Comment);
                return Ok("Review submitted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
