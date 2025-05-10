using BookHaven.Dtos;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/publisher")]
    public class PublisherController(IPublisherService publisherService) : Controller
    {
        [HttpPost("addPublisher")]
        public IActionResult AddPublisher([FromBody] AddPublisherDto publisherDto)
        {
            try
            {
                publisherService.AddPublisher(publisherDto);
                return Ok("Publisher added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }
