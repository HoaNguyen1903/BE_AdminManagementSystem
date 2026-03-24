using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
