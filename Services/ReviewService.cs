using BookHaven.DTOs;
using BookHaven.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookHaven.Services.Interface;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SubmitReviewAsync(string userId, ReviewDto reviewDto)
    {
        var orderItem = await _context.OrderItems
            .Where(oi => oi.Order.ApplicationUserId == userId && oi.BookID == reviewDto.BookID)
            .FirstOrDefaultAsync();

        if (orderItem == null) return false; // Only allow review for purchased books

        var review = new Review
        {
            ApplicationUserId = userId,
            BookID = reviewDto.BookID,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            ReviewDate = DateTime.Now
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return true;
    }
}
