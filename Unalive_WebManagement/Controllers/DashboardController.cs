using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class DashboardController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IPlayerOnlineHistoryRepository _historyRepository;
        private readonly IUserRepository _userRepository;

        public DashboardController(
            IAnalyticsService analyticsService,
            IPlayerOnlineHistoryRepository historyRepository,
            IUserRepository userRepository)
        {
            _analyticsService = analyticsService;
            _historyRepository = historyRepository;
            _userRepository = userRepository;
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<IEnumerable<RevenueAnalyticsDto>>> GetRevenueAnalytics(
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end,
            [FromQuery] string groupBy = "day")
        {
            var endDate = end ?? DateTime.UtcNow;
            var startDate = start ?? endDate.AddDays(-30);

            if (groupBy != "day" && groupBy != "month")
            {
                return BadRequest("groupBy must be 'day' or 'month'");
            }

            var result = await _analyticsService.GetRevenueAnalyticsAsync(startDate, endDate, groupBy);
            return Ok(result);
        }

        [HttpGet("bundle-ranking")]
        public async Task<ActionResult<IEnumerable<BundleRankingDto>>> GetBundleRanking(
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end,
            [FromQuery] int top = 5)
        {
            var endDate = end ?? DateTime.UtcNow;
            var startDate = start ?? endDate.AddDays(-30);

            var result = await _analyticsService.GetBundleRankingAsync(startDate, endDate, top);
            return Ok(result);
        }

        [HttpGet("player-stats")]
        public async Task<ActionResult<PlayerStatsDto>> GetPlayerStats(
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end)
        {
            var endDate = end ?? DateTime.UtcNow;
            var startDate = start ?? endDate.AddDays(-30);

            var result = await _analyticsService.GetPlayerStatsAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("top-spenders")]
        public async Task<ActionResult<IEnumerable<TopSpenderDto>>> GetTopSpenders([FromQuery] int top = 10)
        {
            var result = await _analyticsService.GetTopSpendersAsync(top);
            return Ok(result);
        }

        [HttpGet("player-history")]
        public async Task<ActionResult<IEnumerable<PlayerOnlineHistoryDto>>> GetPlayerHistory([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var end = endDate ?? DateTime.UtcNow;
            var start = startDate ?? end.AddDays(-30);

            var history = await _historyRepository.GetByDateRangeAsync(start, end);
            
            var result = history.Select(h => new PlayerOnlineHistoryDto
            {
                Date = h.Date,
                OnlineCount = h.OnlineCount,
                DailyActiveUsers = h.DailyActiveUsers
            });

            return Ok(result);
        }

        [HttpGet("current-stats")]
        public async Task<IActionResult> GetCurrentStats()
        {
            var now = DateTime.UtcNow;
            var today = now.Date;
            var onlineThreshold = now.AddMinutes(-5);

            var onlineCount = await _userRepository.CountOnlineUsersAsync(onlineThreshold);
            var dauCount = await _userRepository.CountDailyActiveUsersAsync(today);

            return Ok(new
            {
                CurrentOnline = onlineCount,
                DailyActiveUsers = dauCount,
                Timestamp = now
            });
        }
    }
}
