namespace BookHaven.Services.Interface
{
    
        public interface IReviewService
        {
            void AddReview(string userId, int bookId, int rating, string comment);
        }

    
}
