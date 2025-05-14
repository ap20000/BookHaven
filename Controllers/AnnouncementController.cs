using BookHaven.DTOs;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/annoucement")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementRequestDto request)
        {
            if (request == null) return BadRequest("Invalid announcement details.");

            var announcementId = await _announcementService.CreateAnnouncementAsync(request);
            return CreatedAtAction(nameof(GetActiveAnnouncements), new { id = announcementId }, new { Id = announcementId });
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<AnnouncementResponseDto>>> GetActiveAnnouncements()
        {
            var activeAnnouncements = await _announcementService.GetActiveAnnouncementsAsync();
            return Ok(activeAnnouncements);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var success = await _announcementService.DeleteAnnouncementAsync(id);
            if (!success) return NotFound("Announcement not found.");

            return NoContent();
        }
    }
}
