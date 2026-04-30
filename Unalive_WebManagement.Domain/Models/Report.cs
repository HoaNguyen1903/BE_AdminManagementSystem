using System;

namespace Unalive_WebManagement.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int SenderId { get; set; }

    public int AccusedId { get; set; }

    public string Reason { get; set; } = null!;

    public string? AdditionalInfo { get; set; }

    public DateTime SendDate { get; set; }

    public short Approved { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int? ApprovedBy { get; set; }
}
