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
    public string? BundleName { get; set; }
    public string? ItemName { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    public long Price { get; set; }

    // public int BundleId { get; set; } --> Map back to Bundle if needed down the road
}