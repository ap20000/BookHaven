using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SubmitReviewAsync(string userId, ReviewDto reviewDto)
    {
        // Ensure the user has purchased the book
        var orderItem = await _context.OrderItems
            .Where(oi => oi.Order.ApplicationUserId == userId && oi.BookID == reviewDto.BookID)
            .FirstOrDefaultAsync();

        // If the user hasn't purchased the book, return false
        if (orderItem == null) return false;

        // Create a new Review object
        var review = new Review
        {
            ApplicationUserId = userId,
            BookID = reviewDto.BookID,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            ReviewDate = DateTime.UtcNow // Ensure ReviewDate is in UTC
        };

        // Add the review to the database
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return true; // Successfully saved the review
    }

    public async Task<List<Review>> GetReviewsByBookIdAsync(int bookId)
    {
        return await _context.Reviews
            .Where(r => r.BookID == bookId)
            .Include(r => r.ApplicationUser)  // Include user information for username
            .ToListAsync();
    }
}