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

        [HttpGet("getAll")]
        public IActionResult GetAllPublishers()
        {
            try
            {
                var publishers = publisherService.GetAllPublishers();
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeletePublisher(int id)
        {
            try
            {
                var deleted = publisherService.DeletePublisher(id);
                if (!deleted)
                    return NotFound("Publisher not found.");
                return Ok("Publisher deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }
