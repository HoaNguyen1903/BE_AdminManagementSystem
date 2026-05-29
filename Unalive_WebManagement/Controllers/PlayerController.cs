using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.BLL.Services;
using Unalive_WebManagement.Domain.DTOs;
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
        private readonly IBlobService _blobService;
        public PlayerController(
            IUserService userService,
            IAnnouncementService announcementService,
            IShopService shopService,
            IFeedbackService feedbackService,
            ICharacterService characterService,
            IBlobService blobService)
        {
            _userService = userService;
            _announcementService = announcementService;
            _shopService = shopService;
            _feedbackService = feedbackService;
            _characterService = characterService;
            _blobService = blobService;
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

        [HttpPost("avatar")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Only JPEG, PNG and WebP are allowed");

            if (file.Length > 2 * 1024 * 1024)
                return BadRequest("File size cannot exceed 2MB");

            var userId = GetAuthenticatedUserId();
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"avatars/user/{userId}/{Guid.NewGuid()}{extension}";

            var url = await _blobService.UploadImageAsync(file, fileName);
            await _userService.UpdateAvatarAsync(userId, url);

            return Ok(new ImageUrl { AvatarUrl = url });
        }
        class ImageUrl
        {
            public string AvatarUrl { get; set; }
        }

        private int GetAuthenticatedUserId()
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(nameIdentifier, out var idFromNameIdentifier)) return idFromNameIdentifier;

            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue("sub");
            if (int.TryParse(sub, out var idFromSub)) return idFromSub;

            throw new UnauthorizedAccessException("User id claim is missing.");
        }
        [HttpGet("rankingLeaderBoard")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetRanksByStartIndexAndEndIndex([FromQuery] int limit = 10, [FromQuery] string direction = "around", [FromQuery] int cursorRank = 0)
        {
            var users = await _userService.GetUsersAsync(new UserFilterParameters());
            var sortedUserByRankPoint = users.OrderByDescending(x => x.RankPoint).ToList();
            var totalPlayers = sortedUserByRankPoint.Count;
            long startIndex = 0;
            long stopIndex = 0;
            switch (direction.ToLower())
            {
                case "around":
                    var myRankIndex = sortedUserByRankPoint.FindIndex(x => x.UserId == CurrentPlayerId);
                    var fiveIndexAbove = myRankIndex - 5;
                    var fourIndexBelow = startIndex + limit - 1;
                    startIndex = Math.Max(0, fiveIndexAbove);
                    stopIndex = Math.Min(totalPlayers - 1, fourIndexBelow);
                    break;
                case "up":
                    long targetStopIndex = cursorRank - 2;
                    startIndex = Math.Max(0, targetStopIndex - limit + 1);
                    stopIndex = targetStopIndex;
                    break;
                case "down":
                    startIndex = cursorRank;
                    stopIndex = Math.Min(totalPlayers - 1, startIndex + limit - 1);
                    break;
            }
            if (startIndex > stopIndex || startIndex >= totalPlayers)
            {
                return Ok(new LeaderboardResponse { Message = "Không còn dữ liệu" });
            }
            var currentRank = startIndex;
            var rankings = sortedUserByRankPoint
                .Skip((int)(startIndex + 1))
                .Take((int)(stopIndex - startIndex - 1))
                .Select((user, index) => new PlayerRankDto
                {
                    Rank = currentRank + index + 1,
                    DisplayName = $"{user.FirstName} {user.LastName}",
                    RankPoint = user.RankPoint,
                    UserId = user.UserId,
                    AvatarUrl = user.AvatarUrl
                })
                 .ToList();
            var response = new LeaderboardResponse
            {
                Meta = new LeaderboardMeta
                {
                    Direction = direction,
                    HasBefore = startIndex > 0,
                    HasAfter = stopIndex < (totalPlayers - 1),
                    TopRank = startIndex + 1,
                    BottomRank = stopIndex + 1,
                },
                Data = new LeaderboardData
                {
                    Rankings = rankings
                }
            };
            return Ok(response);
        }
    }
}
