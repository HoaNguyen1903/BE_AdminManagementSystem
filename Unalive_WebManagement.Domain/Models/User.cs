using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateTime? Banned { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Report> ReportAccuseds { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportSenders { get; set; } = new List<Report>();

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
