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

        // UserItem Endpoints
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetAllUserItems([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetAllUserItemsAsync(query));
        }

        [HttpGet("{userId}/items")]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetUserItems(int userId, [FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserItemsByUserIdAsync(userId, query));
        }

        [HttpPost("items")]
        public async Task<ActionResult<UserItemDto>> CreateUserItem([FromBody] CreateUserItemDto dto)
        {
            var created = await _userService.CreateUserItemAsync(dto);
            return Ok(created);
        }

        [HttpPut("{userId}/items/{itemId}")]
        public async Task<IActionResult> UpdateUserItem(int userId, int itemId, [FromBody] UpdateUserItemDto dto)
        {
            try
            {
                await _userService.UpdateUserItemAsync(userId, itemId, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{userId}/items/{itemId}")]
        public async Task<IActionResult> DeleteUserItem(int userId, int itemId)
        {
            await _userService.DeleteUserItemAsync(userId, itemId);
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
