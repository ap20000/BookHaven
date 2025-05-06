using BookHaven.Dtos;
using BookHaven.Services;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHaven.Controllers
{
    [Authorize]
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
    }

}
