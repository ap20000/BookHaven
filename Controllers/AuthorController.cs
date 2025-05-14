using BookHaven.Dtos;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService.GetAllAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            return author != null ? Ok(author) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto dto)
        {
            var created = await _authorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.AuthorID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorCreateDto dto)
        {
            var success = await _authorService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _authorService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

}
