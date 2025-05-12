using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/user-actions")]
    public class UserActionsController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly ICartService _cartService;

        public UserActionsController(IBookmarkService bookmarkService, ICartService cartService)
        {
            _bookmarkService = bookmarkService;
            _cartService = cartService;
        }

        [HttpPost("bookmark/{userId}/{bookId}")]
        public IActionResult AddBookmark(string userId, int bookId)
        {
            try
            {
                _bookmarkService.AddBookmark(userId, bookId);
                return Ok("Book added to your bookmarks (whitelist).");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cart/{userId}/{bookId}")]
        public IActionResult AddToCart(string userId, int bookId, [FromQuery] int quantity = 1)
        {
            try
            {
                _cartService.AddToCart(userId, bookId, quantity);
                return Ok("Book added to your cart.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cart/{userId}")]
        public IActionResult GetCartItems(string userId)
        {
            try
            {
                var cartItems = _cartService.GetCartItems(userId);

                var cartDto = cartItems.Select(item => new
                {
                    bookId = item.BookID,
                    title = item.Book.Title,
                    price = item.Book.Price,
                    quantity = item.Quantity,
                    total = item.Quantity * item.Book.Price,
                    // coverImageUrl = item.Book.CoverImageUrl // if available
                });

                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving cart items: {ex.Message}");
            }
        }
    }
}
