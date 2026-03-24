using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BannerItemController : ControllerBase
    {
        private readonly IBannerItemService _bannerItemService;

        public BannerItemController(IBannerItemService bannerItemService)
        {
            _bannerItemService = bannerItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BannerItemDto>>> GetAll()
        {
            return Ok(await _bannerItemService.GetAllBannerItemsAsync());
        }

        [HttpGet("{bannerId}/{itemId}")]
        public async Task<ActionResult<BannerItemDto>> GetById(int bannerId, int itemId)
        {
            var bannerItem = await _bannerItemService.GetBannerItemByIdAsync(bannerId, itemId);
            if (bannerItem == null) return NotFound();
            return Ok(bannerItem);
        }

        [HttpPost]
        public async Task<ActionResult<BannerItemDto>> Create([FromBody] CreateBannerItemDto dto)
        {
            var created = await _bannerItemService.CreateBannerItemAsync(dto);
            return CreatedAtAction(nameof(GetById), new { bannerId = created.BannerId, itemId = created.ItemId }, created);
        }

        [HttpDelete("{bannerId}/{itemId}")]
        public async Task<IActionResult> Delete(int bannerId, int itemId)
        {
            await _bannerItemService.DeleteBannerItemAsync(bannerId, itemId);
            return NoContent();
        }
    }
}
