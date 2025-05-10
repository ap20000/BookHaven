using Microsoft.AspNetCore.Mvc;
using static BookHaven.Services.BookService;
using System.Security.Claims;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/user-actions")]
    public class UserActionsController : ControllerBase
    {
        private readonly BookmarkService _bookmarkService;
        private readonly CartService _cartService;

        public UserActionsController(BookmarkService bookmarkService, CartService cartService)
        {
            _bookmarkService = bookmarkService;
            _cartService = cartService;
        }

        [HttpPost("bookmark/{bookId}")]
        public IActionResult AddBookmark(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _bookmarkService.AddBookmark(userId, bookId);
            return Ok("Book bookmarked.");
        }

        [HttpPost("cart/{bookId}")]
        public IActionResult AddToCart(int bookId, [FromQuery] int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.AddToCart(userId, bookId, quantity);
            return Ok("Book added to cart.");
        }
    }
}
