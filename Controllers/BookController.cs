using System.Security.Claims;
using BookHaven.Dtos;
using BookHaven.Services;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using static BookHaven.Services.BookService;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController(IBookService bookService) : Controller
    {
        [HttpPost("addBook")]
        public IActionResult AddBook([FromBody] AddBookDto bookDto)
        {
            try
            {
                bookService.AddBook(bookDto);
                return Ok("Book added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllBook")]
        public IActionResult GetAllBook()
        {
            try
            {
                var result = bookService.GetAllBooks();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = bookService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                bookService.DeleteBook(id);
                return Ok("Book deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto bookDto)
        {
            try
            {
                bookService.UpdateBook(id, bookDto);
                return Ok("Book updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("catalog")]
        public IActionResult BrowseCatalog(
            string? search = null,
            string? sortBy = null,
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            try
            {
                  var result = bookService.BrowseCatalog(search, sortBy, pageNumber, pageSize);
                  return Ok(result);
            }
            catch (Exception ex)
            {
              return BadRequest(ex.Message);
            }
        }

        
        
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

}
