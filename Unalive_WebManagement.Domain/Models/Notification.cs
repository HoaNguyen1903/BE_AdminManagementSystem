using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int ReceiverId { get; set; }

    public string NotificationMessage { get; set; } = null!;

    public DateOnly SentDate { get; set; }

    public virtual Staff Receiver { get; set; } = null!;
}
