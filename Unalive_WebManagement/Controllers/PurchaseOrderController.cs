using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        // GET: api/PurchaseOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetAllPurchaseOrders()
        {
            try
            {
                var orders = await _purchaseOrderService.GetAllPurchaseOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching purchase orders.", details = ex.Message });
            }
        }

        // GET: api/PurchaseOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderById(int id)
        {
            var order = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/PurchaseOrder
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDto dto)
        {
            try
            {
                var createdOrder = await _purchaseOrderService.ProcessBundlePurchaseAsync(dto.UserId, dto.SkinAndCharacterBundleId);

                return CreatedAtAction(
                    nameof(GetPurchaseOrderById),
                    new { id = createdOrder.PurchaseOrderId },
                    createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/PurchaseOrder/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(int id, [FromBody] PurchaseOrder order)
        {
            try
            {
                await _purchaseOrderService.UpdatePurchaseOrderAsync(id, order);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/PurchaseOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            try
            {
                await _purchaseOrderService.DeletePurchaseOrderAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}