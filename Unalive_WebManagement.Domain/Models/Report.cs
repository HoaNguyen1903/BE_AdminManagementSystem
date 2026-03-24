using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int SenderId { get; set; }

    public int AccusedId { get; set; }

    public string Reason { get; set; } = null!;

    public string? AdditionalInfo { get; set; }

    public DateTime SendDate { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public virtual User Accused { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;

    public virtual Staff? ApprovedByNavigation { get; set; }
}
