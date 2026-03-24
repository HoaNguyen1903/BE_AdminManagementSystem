using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BannerDto>>> GetAll()
        {
            return Ok(await _bannerService.GetAllBannersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BannerDto>> GetById(int id)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id);
            if (banner == null) return NotFound();
            return Ok(banner);
        }

        [HttpPost]
        public async Task<ActionResult<BannerDto>> Create([FromBody] CreateBannerDto dto)
        {
            var created = await _bannerService.CreateBannerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.BannerId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBannerDto dto)
        {
            try
            {
                await _bannerService.UpdateBannerAsync(id, dto);
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
            await _bannerService.DeleteBannerAsync(id);
            return NoContent();
        }
    }
}
