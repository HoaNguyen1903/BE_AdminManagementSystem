namespace Unalive_WebManagement.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateOnly OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
