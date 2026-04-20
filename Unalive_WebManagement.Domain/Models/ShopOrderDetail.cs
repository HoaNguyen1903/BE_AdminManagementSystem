using System;

namespace Unalive_WebManagement.Models;

public partial class ShopOrderDetail
{
    public int ShopOrderDetailId { get; set; }

    public int ShopOrderId { get; set; }

    public int? SkinAndCharacterBundleId { get; set; }

    public int? GemBundleId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public float UnitPrice { get; set; }
}
