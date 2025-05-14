using System.Security.Claims;
using BookHaven.Dtos;
using BookHaven.Services;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = bookService.GetById(id);
            if (book == null)
                return NotFound("Book not found");

            return Ok(book);
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


        [HttpGet("category/{category}")]
        public IActionResult GetBooksByCategory(string category, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = bookService.GetBooksByCategory(category, pageNumber, pageSize);
                return Ok(result); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }

}
