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

    public void AddReview(string userId, int bookId, int rating, string comment)
    {
        var hasPurchased = _context.Purchases
            .Any(p => p.ApplicationUserId == userId && p.BookID == bookId);

        if (!hasPurchased)
            throw new Exception("You can only review books you've purchased.");

        var review = new Review
        {
            ApplicationUserId = userId,
            BookID = bookId,
            Rating = rating,
            Comment = comment,
            ReviewDate = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        _context.SaveChanges();
    }
}
