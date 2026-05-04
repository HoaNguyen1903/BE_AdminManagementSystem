using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unalive_WebManagement.Models
{
    public partial class PurchaseOrder
    {
        // BASIC ORDER INFO
        public int PurchaseOrderId { get; set; }
        public DateTimeOffset PurchaseDate { get; set; } = DateTimeOffset.UtcNow;

        // CUSTOMER INFORMATION
        public int UserId { get; set; }

        // BUNDLE INFORMATION
        public int SkinAndCharacterBundleId { get; set; }

        // TRANSACTION AMOUNT
        public float GemCost { get; set; }

        // ORDER STATUS
        public InGamePurchaseStatus Status { get; set; } = InGamePurchaseStatus.Pending;

        [ForeignKey(nameof(SkinAndCharacterBundleId))]
        public SkinAndCharacterBundle? Bundle { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }

    public enum InGamePurchaseStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}