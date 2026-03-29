using System;

namespace Unalive_WebManagement.Models;

public partial class TopUpHistory
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
