using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GiftController : ControllerBase
    {
        private readonly IUserService _userService;

        public GiftController(IUserService userService) { 
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserItemNoOrderDto dto)
        {
            var created = await _userService.CreateUserItemNoOrderAsync(dto);
            return Ok(created);
        }
    }
}
