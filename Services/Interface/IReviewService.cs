using BookHaven.DTOs;
namespace BookHaven.Services.Interface
{
    public interface IReviewService
    {
        Task<bool> SubmitReviewAsync(string userId, ReviewDto reviewDto);
    }
}
