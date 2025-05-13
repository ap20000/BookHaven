using BookHaven.Entities;

namespace BookHaven.Services.Interface
{
    public interface ICartService
    {
        // Adds a book to the user's cart with a specified quantity
        void AddToCart(string userId, int bookId, int quantity);

        // Retrieves all cart items for a user
        IEnumerable<CartItem> GetCartItems(string userId);
    }
}
