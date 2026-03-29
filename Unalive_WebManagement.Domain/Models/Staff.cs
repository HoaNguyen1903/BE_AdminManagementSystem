using System;

namespace Unalive_WebManagement.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}
