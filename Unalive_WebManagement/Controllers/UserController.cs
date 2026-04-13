using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetAllUsersAsync(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
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

        [HttpDelete("{id}")]
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
    }
}
