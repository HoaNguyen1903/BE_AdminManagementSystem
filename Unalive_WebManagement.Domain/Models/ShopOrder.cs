using PayOS.Models.V2.PaymentRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Models;

public partial class ShopOrder
{
    // BASIC ORDER INFORMATION
    public int ShopOrderId { get; set; }
    public float TotalAmount { get; set; }
    public long OrderCode { get; set; }

    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

    // AMOUNT TRACKING
    public long Amount { get; set; }
    public long AmountPaid { get; set; } = 0;
    public long AmountRemaining { get; set; } = 0;

    // CUSTOMER INFORMATION
    public int UserId { get; set; }
    public string? PlayerEmail { get; set; }
    public string? PlayerUserName { get; set; }

    // PAYMENT LINK RELATED PROPERTIES
    public string? PaymentLinkId { get; set; }
    public string? QrCode { get; set; }
    public string? CheckoutUrl { get; set; }
    public PaymentLinkStatus Status { get; set; } = PaymentLinkStatus.Pending;

    // PAYMENT LINK DETAILS
    public string? Bin { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public string? Currency { get; set; } = "VND";

    // URLs
    public string? ReturnUrl { get; set; }
    public string? CancelUrl { get; set; }

    // TIMESTAMPS (if needed)
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? CanceledAt { get; set; }
    public DateTimeOffset? ExpiredAt { get; set; }
    public DateTimeOffset? LastTransactionUpdate { get; set; }

    // Cancellation
    public string? CancellationReason { get; set; }

    [JsonIgnore]
    public List<ShopOrderDetail> OrderDetails { get; set; } = new List<ShopOrderDetail>();

    [NotMapped]
    [JsonPropertyName("orderDetails")]
    public IEnumerable<ShopOrderDetailDto>? DetailedOrderDetails { get; set; }

    [NotMapped]
    public short IsSuccess => (short)(Status == PaymentLinkStatus.Paid ? 1 : 0);
}
