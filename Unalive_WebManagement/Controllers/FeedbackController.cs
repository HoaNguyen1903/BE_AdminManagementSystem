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

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports([FromQuery] QueryParameters query)
        {
            return Ok(await _feedbackService.GetAllReportsAsync(query));
        }

        [HttpGet("reports/{id}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await _feedbackService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }
    }
}
