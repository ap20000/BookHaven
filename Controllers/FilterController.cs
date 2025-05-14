using BookHaven.Dtos;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class FilterController : ControllerBase
{
    private readonly IFilterService _giveService;

    public FilterController(IFilterService giveService)
    {
        _giveService = giveService;
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetBooks([FromQuery] BookFilterDto filter)
    {
        var books = await _giveService.GetFilteredBooksAsync(filter);
        return Ok(books);
    }
}