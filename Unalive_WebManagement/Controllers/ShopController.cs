using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("gem-bundles")]
        public async Task<ActionResult<IEnumerable<GemBundleDto>>> GetGemBundles()
        {
            return Ok(await _shopService.GetAllGemBundlesAsync());
        }

        [HttpGet("skin-and-character-bundles")]
        public async Task<ActionResult<IEnumerable<SkinAndCharacterBundleDto>>> GetSkinAndCharacterBundles()
        {
            return Ok(await _shopService.GetAllSkinAndCharacterBundlesAsync());
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<ShopOrderDto>>> GetOrders()
        {
            return Ok(await _shopService.GetAllShopOrdersAsync());
        }

        [HttpGet("orders/{orderId}/details")]
        public async Task<ActionResult<IEnumerable<ShopOrderDetailDto>>> GetOrderDetails(int orderId)
        {
            return Ok(await _shopService.GetOrderDetailsByOrderIdAsync(orderId));
        }

        [HttpGet("topup-history")]
        public async Task<ActionResult<IEnumerable<TopUpHistoryDto>>> GetTopUpHistory()
        {
            return Ok(await _shopService.GetAllTopUpHistoriesAsync());
        }
    }
}
