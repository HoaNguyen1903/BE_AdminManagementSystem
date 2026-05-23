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

        //  CRUD endpoints 

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

        //  SignalR broadcast endpoints 


        [HttpPost("send-create")]
        public async Task<ActionResult<AnnouncementDto>> SendCreate([FromBody] CreateAnnouncementDto dto)
        {
            if (dto == null) return BadRequest("Payload không được để trống.");

            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var created = await _announcementService.CreateAnnouncementAsync(dto, staffId);

            await _hubContext.Clients.All.SendAsync("AnnouncementCreated", created);

            return CreatedAtAction(nameof(GetById), new { id = created.AnnouncementId }, created);
        }


        [HttpPut("send-update/{id}")]
        public async Task<IActionResult> SendUpdate(int id, [FromBody] UpdateAnnouncementDto dto)
        {
            if (dto == null) return BadRequest("Payload không được để trống.");

            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                var updated = await _announcementService.UpdateAnnouncementAsync(id, dto, staffId);

                await _hubContext.Clients.All.SendAsync("AnnouncementUpdated", updated);

                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Không tìm thấy announcement với id = {id}.");
            }
        }


        [HttpDelete("send-delete/{id}")]
        public async Task<IActionResult> SendDelete(int id)
        {
            var existing = await _announcementService.GetAnnouncementByIdAsync(id);
            if (existing == null) return NotFound($"Không tìm thấy announcement với id = {id}.");

            await _announcementService.DeleteAnnouncementAsync(id);

            await _hubContext.Clients.All.SendAsync("AnnouncementDeleted", new { announcementId = id });

            return Ok(new { message = $"Announcement {id} đã được xóa và thông báo đến game server." });
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendNewAnnoucement([FromBody] CreateAnnouncementDto dto)
        {
            if (dto == null) return BadRequest();
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var payload = new AnnouncementDto
            {
                AnnouncementId = 0,
                Title = dto.Title,
                Content = dto.Content,
                Type = dto.Type,
                Status = dto.Status,
                CreatedBy = staffId,
                UpdatedBy = null
            };
            await _hubContext.Clients.All.SendAsync("SendNewAnnoucement", payload);
            return Ok(payload);
        }
    }
}