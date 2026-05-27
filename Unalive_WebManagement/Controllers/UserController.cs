using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.BLL.Services;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBlobService _blobService;

        public UserController(IUserService userService, IBlobService blobService)
        {
            _userService = userService;
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] UserFilterParameters filter)
        {
            return Ok(await _userService.GetUsersAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<PlayerProfileDto>> GetProfile(int id)
        {
            var profile = await _userService.GetPlayerProfileAsync(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<UserItemWithNameDto>>> GetUserItems(int id, [FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserItemsWithNamesByUserIdAsync(id, query));
        }

        [HttpGet("{id}/rank-point")]
        public async Task<ActionResult<long>> GetRankPoint(int id)
        {
            try
            {
                var rankPoint = await _userService.GetRankPointAsync(id);
                return Ok(rankPoint);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/rank-point")]
        public async Task<ActionResult<long>> UpdateRankPoint(int id, [FromBody] UpdateRankPointRequest request)
        {
            try
            {
                var newRankPoint = await _userService.UpdateRankPointAsync(id, request.Points, request.IsAddition);
                return Ok(newRankPoint);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                var created = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                await _userService.UpdateUserAsync(id, dto);
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

        [HttpPost("{id}/ban")]
        public async Task<IActionResult> Ban(int id, [FromBody] BanUserRequest dto)
        {
            var staffId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                await _userService.BanUserAsync(id, dto, staffId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/permanent-ban")]
        public async Task<IActionResult> PermanentBan(int id, [FromBody] string reason)
        {
            var staffId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                await _userService.PermanentBanUserAsync(id, reason, staffId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("me/heartbeat")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Heartbeat()
        {
            var userId = GetAuthenticatedUserId();
            await _userService.UpdateUserLastOnlineAsync(userId);
            return NoContent();
        }

        [HttpPost("me/logout")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Logout()
        {
            var userId = GetAuthenticatedUserId();
            await _userService.SetUserOfflineAsync(userId);
            return NoContent();
        }

        [HttpGet("{id}/status")]
        [AllowAnonymous] // Keep this AllowAnonymous or specific logic if needed, but previously it was open
        public async Task<ActionResult<UserStatusDto>> GetStatus(int id, [FromQuery] int onlineThresholdSeconds = 120)
        {
            if (onlineThresholdSeconds <= 0) onlineThresholdSeconds = 120;

            // Internal logic still checks roles
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("User"))
            {
                var currentUserId = GetAuthenticatedUserId();
                if (currentUserId != id) return Forbid();
            }

            var status = await _userService.GetUserStatusAsync(id, onlineThresholdSeconds);
            if (status == null) return NotFound();
            return Ok(status);
        }

        // UserBanLog Endpoints
        [HttpGet("ban-logs")]
        public async Task<ActionResult<IEnumerable<UserBanLogDto>>> GetAllUserBanLogs([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetAllUserBanLogsAsync(query));
        }

        [HttpGet("{userId}/ban-logs")]
        public async Task<ActionResult<IEnumerable<UserBanLogDto>>> GetUserBanLogs(int userId, [FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserBanLogsByUserIdAsync(userId, query));
        }

        [HttpPost("ban-logs")]
        public async Task<ActionResult<UserBanLogDto>> CreateUserBanLog([FromBody] CreateUserBanLogDto dto)
        {
            var staffId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var created = await _userService.CreateUserBanLogAsync(dto, staffId);
            return Ok(created);
        }

        [HttpPut("ban-logs/{id}")]
        public async Task<IActionResult> UpdateUserBanLog(int id, [FromBody] UpdateUserBanLogDto dto)
        {
            try
            {
                await _userService.UpdateUserBanLogAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("ban-logs/{id}")]
        public async Task<IActionResult> DeleteUserBanLog(int id)
        {
            await _userService.DeleteUserBanLogAsync(id);
            return NoContent();
        }

        // UserBundle Endpoints
        [HttpGet("bundles")]
        public async Task<ActionResult<IEnumerable<UserBundleDto>>> GetAllUserBundles([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetAllUserBundlesAsync(query));
        }

        [HttpGet("{userId}/bundles")]
        public async Task<ActionResult<IEnumerable<UserBundleDto>>> GetUserBundles(int userId, [FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserBundlesByUserIdAsync(userId, query));
        }

        [HttpPost("bundles")]
        public async Task<ActionResult<UserBundleDto>> CreateUserBundle([FromBody] CreateUserBundleDto dto)
        {
            var created = await _userService.CreateUserBundleAsync(dto);
            return Ok(created);
        }

        [HttpPut("{userId}/bundles/skin/{skinBundleId}/gem/{gemBundleId}")]
        public async Task<IActionResult> UpdateUserBundle(int userId, int skinBundleId, int gemBundleId, [FromBody] UpdateUserBundleDto dto)
        {
            try
            {
                await _userService.UpdateUserBundleAsync(userId, skinBundleId, gemBundleId, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{userId}/bundles/skin/{skinBundleId}/gem/{gemBundleId}")]
        public async Task<IActionResult> DeleteUserBundle(int userId, int skinBundleId, int gemBundleId)
        {
            await _userService.DeleteUserBundleAsync(userId, skinBundleId, gemBundleId);
            return NoContent();
        }

        private int GetAuthenticatedUserId()
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(nameIdentifier, out var idFromNameIdentifier)) return idFromNameIdentifier;

            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue("sub");
            if (int.TryParse(sub, out var idFromSub)) return idFromSub;

            throw new UnauthorizedAccessException("User id claim is missing.");
        }

        [HttpGet("me/profile")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PlayerProfileDto>> GetMyProfile()
        {
            var userId = GetAuthenticatedUserId();
            var profile = await _userService.GetPlayerProfileAsync(userId);
            if (profile == null) return NotFound();
            return Ok(profile);
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

            return Ok(new { avatarUrl = url });
        }
    }
}
