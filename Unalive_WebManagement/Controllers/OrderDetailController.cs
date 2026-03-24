using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetAll()
        {
            return Ok(await _orderDetailService.GetAllOrderDetailsAsync());
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<ActionResult<OrderDetailDto>> GetById(int orderId, int productId)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(orderId, productId);
            if (orderDetail == null) return NotFound();
            return Ok(orderDetail);
        }
    }
}
