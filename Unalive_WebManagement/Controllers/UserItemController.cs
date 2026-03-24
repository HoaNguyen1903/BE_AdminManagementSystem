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
        private readonly IUserItemService _userItemService;

        public UserItemController(IUserItemService userItemService)
        {
            _userItemService = userItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetAll()
        {
            return Ok(await _userItemService.GetAllUserItemsAsync());
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetByUserId(int userId)
        {
            return Ok(await _userItemService.GetUserItemsByUserIdAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserItemDto>> GetById(int id)
        {
            var userItem = await _userItemService.GetUserItemByIdAsync(id);
            if (userItem == null) return NotFound();
            return Ok(userItem);
        }

        [HttpPost]
        public async Task<ActionResult<UserItemDto>> Create([FromBody] CreateUserItemDto dto)
        {
            var created = await _userItemService.CreateUserItemAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.UserItemId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserItemDto dto)
        {
            try
            {
                await _userItemService.UpdateUserItemAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userItemService.DeleteUserItemAsync(id);
            return NoContent();
        }
    }
}
