using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddToCart(string userId, int bookId, int quantity)
        {
            var existingCartItem = _context.CartItems
                .FirstOrDefault(c => c.ApplicationUserId == userId && c.BookID == bookId);

            if (existingCartItem != null)
            {
                // Update quantity if book is already in the cart
                existingCartItem.Quantity += quantity;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    ApplicationUserId = userId,
                    BookID = bookId,
                    Quantity = quantity
                };

                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges();
        }

        public IEnumerable<CartItem> GetCartItems(string userId)
        {
            return _context.CartItems.Include(c => c.Book)
                                      .Where(c => c.ApplicationUserId == userId)
                                      .ToList();
        }
    }
}

