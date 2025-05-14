using BookHaven.DTOs;
namespace BookHaven.Services.Interface
{
    public interface IReviewService
    {
        Task<bool> SubmitReviewAsync(string userId, ReviewDto reviewDto);
        Task<List<Review>> GetReviewsByBookIdAsync(int bookId); // New method to get reviews by bookId
    }
}
