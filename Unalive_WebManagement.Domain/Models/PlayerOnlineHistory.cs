using System;

namespace Unalive_WebManagement.Models;

public partial class PlayerOnlineHistory
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int OnlineCount { get; set; } // Current/Peak online players in the snapshot
    public int DailyActiveUsers { get; set; } // Unique users who logged in today so far
}
