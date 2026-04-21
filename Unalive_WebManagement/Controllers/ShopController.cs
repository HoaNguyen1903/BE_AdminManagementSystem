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
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        // GemBundle Endpoints
        [HttpGet("gem-bundles")]
        public async Task<ActionResult<IEnumerable<GemBundleDto>>> GetGemBundles([FromQuery] QueryParameters query)
        {
            return Ok(await _shopService.GetAllGemBundlesAsync(query));
        }

        [HttpGet("gem-bundles/{id}")]
        public async Task<ActionResult<GemBundleDto>> GetGemBundleById(int id)
        {
            var b = await _shopService.GetGemBundleByIdAsync(id);
            if (b == null) return NotFound();
            return Ok(b);
        }

        [HttpPost("gem-bundles")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<GemBundleDto>> CreateGemBundle([FromBody] CreateGemBundleDto dto)
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var createdBundle = await _shopService.CreateGemBundleAsync(dto, staffId);
            return CreatedAtAction(nameof(GetGemBundleById), new { id = createdBundle.GemBundleId }, createdBundle);
        }

        [HttpPut("gem-bundles/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateGemBundle(int id, [FromBody] UpdateGemBundleDto dto)
        {
            try
            {
                await _shopService.UpdateGemBundleAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("gem-bundles/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteGemBundle(int id)
        {
            try
            {
                await _shopService.DeleteGemBundleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // SkinAndCharacterBundle Endpoints
        [HttpGet("skin-and-character-bundles")]
        public async Task<ActionResult<IEnumerable<SkinAndCharacterBundleDto>>> GetSkinAndCharacterBundles([FromQuery] QueryParameters query)
        {
            return Ok(await _shopService.GetAllSkinAndCharacterBundlesAsync(query));
        }

        [HttpGet("skin-and-character-bundles/{id}")]
        public async Task<ActionResult<SkinAndCharacterBundleDto>> GetSkinAndCharacterBundleById(int id)
        {
            var b = await _shopService.GetSkinAndCharacterBundleByIdAsync(id);
            if (b == null) return NotFound();
            return Ok(b);
        }

        [HttpPost("skin-and-character-bundles")]
        public async Task<ActionResult<SkinAndCharacterBundleDto>> CreateSkinAndCharacterBundle([FromBody] CreateSkinAndCharacterBundleDto dto)
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var created = await _shopService.CreateSkinAndCharacterBundleAsync(dto, staffId);
            return CreatedAtAction(nameof(GetSkinAndCharacterBundleById), new { id = created.SkinAndCharacterBundleId }, created);
        }

        [HttpPut("skin-and-character-bundles/{id}")]
        public async Task<IActionResult> UpdateSkinAndCharacterBundle(int id, [FromBody] UpdateSkinAndCharacterBundleDto dto)
        {
            try
            {
                await _shopService.UpdateSkinAndCharacterBundleAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("skin-and-character-bundles/{id}")]
        public async Task<IActionResult> DeleteSkinAndCharacterBundle(int id)
        {
            await _shopService.DeleteSkinAndCharacterBundleAsync(id);
            return NoContent();
        }

        // ShopOrder & Details & TopUp Endpoints
        //[HttpGet("orders")]
        //public async Task<ActionResult<IEnumerable<ShopOrderDto>>> GetOrders([FromQuery] QueryParameters query)
        //{
        //    return Ok(await _shopService.GetAllShopOrdersAsync(query));
        //}

        //[HttpGet("orders/{orderId}/details")]
        //public async Task<ActionResult<IEnumerable<ShopOrderDetailDto>>> GetOrderDetails(int orderId, [FromQuery] QueryParameters query)
        //{
        //    return Ok(await _shopService.GetOrderDetailsByOrderIdAsync(orderId, query));
        //}

        //[HttpGet("topup-history")]
        //public async Task<ActionResult<IEnumerable<TopUpHistoryDto>>> GetTopUpHistory([FromQuery] QueryParameters query)
        //{
        //    return Ok(await _shopService.GetAllTopUpHistoriesAsync(query));
        //}
    }
}
