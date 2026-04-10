using System;

namespace Unalive_WebManagement.Models;

public partial class UserBanLog
{
    public int UserBanLogId { get; set; }
    public int UserId { get; set; }
    public string BanReason { get; set; } = null!;
    public DateTime BannedDate { get; set; }
    public DateTime? BannedUntil { get; set; }
    public int BannedBy { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Staff BannedByNavigation { get; set; } = null!;
}
