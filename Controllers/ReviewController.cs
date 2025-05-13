using BookHaven.DTOs;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        // Extract the userId from the JWT claims
        var userId = User.FindFirstValue("userId");

        // Check if the userId exists
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        // Validate the reviewDto
        if (reviewDto.Rating <= 0 || reviewDto.Rating > 5 || string.IsNullOrEmpty(reviewDto.Comment))
        {
            return BadRequest("Invalid review data.");
        }

        // Submit the review
        var result = await _reviewService.SubmitReviewAsync(userId, reviewDto);

        if (!result)
        {
            return BadRequest("You can only review books you have purchased.");
        }

        return Ok("Review submitted successfully.");
    }

    [HttpGet("book/{bookId}")]
    public async Task<IActionResult> GetReviews(int bookId)
    {
        // Get the reviews for the specified book
        var reviews = await _reviewService.GetReviewsByBookIdAsync(bookId);

        // Return an empty list if no reviews are found
        if (reviews == null || reviews.Count == 0)
        {
            return Ok(new List<ReviewDto>());
        }

        // Map the reviews to ReviewDto
        var reviewDtos = reviews.Select(r => new ReviewDto
        {
            ReviewID = r.ReviewID,
            UserID = r.ApplicationUserId,
            Rating = r.Rating,
            Comment = r.Comment,
            ReviewDate = r.ReviewDate
        }).ToList();

        // Return the list of reviews
        return Ok(reviewDtos);
    }
}