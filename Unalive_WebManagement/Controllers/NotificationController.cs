using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAll()
        {
            return Ok(await _notificationService.GetAllNotificationsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return Ok(notification);
        }
    }
}
