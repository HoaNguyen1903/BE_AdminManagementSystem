using System;

namespace Unalive_WebManagement.Models;

public partial class UserItem
{
    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public int? ShopOrderId { get; set; }
    public int? PurchaseOrderId { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Item? Item { get; set; }
    public virtual ShopOrder? ShopOrder { get; set; }
}
