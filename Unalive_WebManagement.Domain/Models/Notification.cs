using System;

namespace Unalive_WebManagement.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string NotificationMessage { get; set; } = null!;

    public int ReceiverId { get; set; }

    public bool Read { get; set; }
}
