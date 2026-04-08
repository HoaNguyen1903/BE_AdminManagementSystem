using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAll([FromQuery] QueryParameters query)
        {
            return Ok(await _announcementService.GetAllAnnouncementsAsync(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnnouncementDto>> GetById(int id)
        {
            var a = await _announcementService.GetAnnouncementByIdAsync(id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<AnnouncementDto>> Create([FromBody] CreateAnnouncementDto dto)
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var created = await _announcementService.CreateAnnouncementAsync(dto, staffId);
            return CreatedAtAction(nameof(GetById), new { id = created.AnnouncementId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                await _announcementService.UpdateAnnouncementAsync(id, dto, staffId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _announcementService.DeleteAnnouncementAsync(id);
            return NoContent();
        }
    }
}
