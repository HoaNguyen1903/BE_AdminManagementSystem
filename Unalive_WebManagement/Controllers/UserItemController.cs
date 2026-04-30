using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserItemController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserItemController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetAll([FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetAllUserItemsAsync(query));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserItemWithNameDto>>> GetByUserId(int userId, [FromQuery] QueryParameters query)
        {
            return Ok(await _userService.GetUserItemsWithNamesByUserIdAsync(userId, query));
        }

        [HttpPost]
        public async Task<ActionResult<UserItemDto>> Create([FromBody] CreateUserItemDto dto)
        {
            var created = await _userService.CreateUserItemAsync(dto);
            return Ok(created);
        }

        [HttpPut("{userId}/{itemId}")]
        public async Task<IActionResult> Update(int userId, int itemId, [FromBody] UpdateUserItemDto dto)
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

        [HttpDelete("{userId}/{itemId}")]
        public async Task<IActionResult> Delete(int userId, int itemId)
        {
            await _userService.DeleteUserItemAsync(userId, itemId);
            return NoContent();
        }
    }
}
