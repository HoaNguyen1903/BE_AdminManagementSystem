using System;

namespace Unalive_WebManagement.Models;

public partial class ShopOrder
{
    public int ShopOrderId { get; set; }

    public int UserId { get; set; }

    public float TotalAmount { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = "Pending";
}
