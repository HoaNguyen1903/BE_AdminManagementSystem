using System;

namespace Unalive_WebManagement.DTOs
{
    public class GemBundleDto
    {
        public int GemBundleId { get; set; }
        public string BundleName { get; set; } = null!;
        public float BundlePrice { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateGemBundleDto
    {
        public string BundleName { get; set; } = null!;
        public float BundlePrice { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateGemBundleDto
    {
        public string? BundleName { get; set; }
        public float? BundlePrice { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
    }

    public class SkinAndCharacterBundleDto
    {
        public int SkinAndCharacterBundleId { get; set; }
        public string BundleName { get; set; } = null!;
        public float BundlePrice { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateSkinAndCharacterBundleDto
    {
        public string BundleName { get; set; } = null!;
        public float BundlePrice { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateSkinAndCharacterBundleDto
    {
        public string? BundleName { get; set; }
        public float? BundlePrice { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
    }

    public class ShopOrderDto
    {
        public int ShopOrderId { get; set; }
        public int UserId { get; set; }
        public float TotalAmount { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public string? PlayerEmail { get; set; }
        public string? PlayerUserName { get; set; }
        public long OrderCode { get; set; }
        public string Currency { get; set; } = null!;
    }

    public class ShopOrderDetailDto
    {
        public int ShopOrderDetailId { get; set; }
        public int ShopOrderId { get; set; }
        public int? SkinAndCharacterBundleId { get; set; }
        public int? GemBundleId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
    }

    public class TopUpHistoryDto
    {
        public int TopUpId { get; set; }
        public int UserId { get; set; }
        public int GemBundleId { get; set; }
        public int GemsAmount { get; set; }
        public float RealMoneyAmount { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string PaymentGateway { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
