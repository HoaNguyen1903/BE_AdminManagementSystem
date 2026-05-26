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
    public class PlayerController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAnnouncementService _announcementService;
        private readonly IShopService _shopService;
        private readonly IFeedbackService _feedbackService;
        private readonly ICharacterService _characterService;

        public PlayerController(
            IUserService userService,
            IAnnouncementService announcementService,
            IShopService shopService,
            IFeedbackService feedbackService,
            ICharacterService characterService)
        {
            _userService = userService;
            _announcementService = announcementService;
            _shopService = shopService;
            _feedbackService = feedbackService;
            _characterService = characterService;
        }

        private int CurrentPlayerId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var user = await _userService.GetUserByIdAsync(CurrentPlayerId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            try
            {
                await _userService.UpdateUserAsync(CurrentPlayerId, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetInventory([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserItemsByUserIdAsync(CurrentPlayerId, query));
        }

        [HttpGet("bundles")]
        public async Task<ActionResult<IEnumerable<UserBundleDto>>> GetBundles([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserBundlesByUserIdAsync(CurrentPlayerId, query));
        }

        [HttpGet("rank-point")]
        public async Task<ActionResult<long>> GetRankPoint()
        {
            return Ok(await _userService.GetRankPointAsync(CurrentPlayerId));
        }

        [HttpPost("rank-point")]
        public async Task<ActionResult<long>> UpdateRankPoint([FromBody] UpdateRankPointRequest request)
        {
            try
            {
                var newRankPoint = await _userService.UpdateRankPointAsync(CurrentPlayerId, request.Points, request.IsAddition);
                return Ok(newRankPoint);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("announcements")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncements([FromQuery] AnnouncementFilterParameters query)
        {
            return Ok(await _announcementService.GetAllAnnouncementsAsync(query));
        }

        [HttpGet("shop/gem-bundles")]
        public async Task<ActionResult<IEnumerable<GemBundleDto>>> GetGemBundles([FromQuery] BundleFilterParameters query)
        {
            return Ok(await _shopService.GetAllGemBundlesAsync(query));
        }

        [HttpGet("shop/skin-bundles")]
        public async Task<ActionResult<IEnumerable<SkinAndCharacterBundleDto>>> GetSkinBundles([FromQuery] BundleFilterParameters query)
        {
            return Ok(await _shopService.GetAllSkinAndCharacterBundlesAsync(query));
        }

        [HttpGet("characters/stats")]
        public async Task<ActionResult<IEnumerable<CharacterStatDto>>> GetCharacterStats([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterStatsAsync(query));
        }

        [HttpGet("characters/pvp")]
        public async Task<ActionResult<IEnumerable<CharacterPvPDto>>> GetCharacterPvPs([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterPvPsAsync(query));
        }

        [HttpPost("shop/buy-gem-bundle/{id}")]
        public async Task<IActionResult> BuyGemBundle(int id)
        {
            try
            {
                await _shopService.PurchaseGemBundleAsync(CurrentPlayerId, id);
                return Ok(new { message = "Purchase successful" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("shop/buy-skin-bundle/{id}")]
        public async Task<IActionResult> BuySkinBundle(int id)
        {
            try
            {
                await _shopService.PurchaseSkinBundleAsync(CurrentPlayerId, id);
                return Ok(new { message = "Purchase successful" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("notifications")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications([FromQuery] QueryParameters query)
        {
            return Ok(await _feedbackService.GetNotificationsByUserIdAsync(CurrentPlayerId, query));
        }

        [HttpPut("notifications/{id}/read")]
        public async Task<IActionResult> MarkNotificationRead(int id)
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

        [HttpPost("report")]
        public async Task<ActionResult<ReportDto>> CreateReport([FromBody] CreateReportDto dto)
        {
            var report = await _feedbackService.CreateReportAsync(dto, CurrentPlayerId);
            return Ok(report);
        }

        [HttpGet("ban-logs")]
        public async Task<ActionResult<IEnumerable<UserBanLogDto>>> GetBanLogs([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserBanLogsByUserIdAsync(CurrentPlayerId, query));
        }
    }
}
