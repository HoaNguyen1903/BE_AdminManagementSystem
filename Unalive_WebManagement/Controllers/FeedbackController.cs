using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("notifications")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAllNotifications([FromQuery] QueryParameters query)
        {
            return Ok(await _feedbackService.GetAllNotificationsAsync(query));
        }

        [HttpPost("notifications/{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            try
            {
                await _feedbackService.MarkNotificationReadAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("notifications/{id}/unread")]
        public async Task<IActionResult> MarkUnread(int id)
        {
            try
            {
                await _feedbackService.MarkNotificationUnreadAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("reports")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports([FromQuery] ReportFilterParameters query)
        {
            return Ok(await _feedbackService.GetAllReportsAsync(query));
        }

        [HttpGet("reports/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await _feedbackService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpPost("reports/{id}/approve")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Approve(int id)
        {
            var staffId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                await _feedbackService.ApproveReportAsync(id, staffId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
