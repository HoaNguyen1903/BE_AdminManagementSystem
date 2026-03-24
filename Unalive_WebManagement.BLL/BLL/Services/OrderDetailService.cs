using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _orderDetailRepository.GetAllAsync();
            return orderDetails.Select(od => new OrderDetailDto
            {
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Subtotal = od.Subtotal
            });
        }

        public async Task<OrderDetailDto?> GetOrderDetailByIdAsync(int orderId, int productId)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(orderId, productId);
            if (orderDetail == null) return null;

            return new OrderDetailDto
            {
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                Subtotal = orderDetail.Subtotal
            };
        }
    }
}
