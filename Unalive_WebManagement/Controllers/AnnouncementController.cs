using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Hubs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        private readonly IHubContext<AnnouncementHub> _hubContext;

        public AnnouncementController(IAnnouncementService announcementService, IHubContext<AnnouncementHub> hubContext)
        {
            _announcementService = announcementService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAll([FromQuery] AnnouncementFilterParameters query)
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
            await _hubContext.Clients.Group("announcements").SendAsync("AnnouncementCreated", created);
            return CreatedAtAction(nameof(GetById), new { id = created.AnnouncementId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var updated = await _announcementService.UpdateAnnouncementAsync(id, dto, staffId);
                await _hubContext.Clients.Group("announcements").SendAsync("AnnouncementUpdated", updated);
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
            await _hubContext.Clients.Group("announcements").SendAsync("AnnouncementDeleted", id);
            return NoContent();
        }
    }
}
