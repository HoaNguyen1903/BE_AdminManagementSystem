using PayOS.Models.V2.PaymentRequests;
using System.ComponentModel.DataAnnotations;

namespace Unalive_WebManagement.Models;

public class OrderCreateRequest
{
    public int UserId { get; set; }
    public float TotalAmount { get; set; } 
    public string? ReturnUrl { get; set; }
    public string? CancelUrl { get; set; }
    public string? PlayerEmail { get; set; }
    public string? PlayerUserName { get; set; }
    public DateTimeOffset? ExpiredAt { get; set; }

    [Required]
    public List<OrderItemCreateRequest> Items { get; set; } = [];
}

public class OrderItemCreateRequest
{
    [Required]
    public int BundleId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int BundleBuyQuantity { get; set; } = 1;
}