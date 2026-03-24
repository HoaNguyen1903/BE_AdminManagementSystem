namespace Unalive_WebManagement.DTOs
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
