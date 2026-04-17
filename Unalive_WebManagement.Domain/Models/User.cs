using System;

namespace Unalive_WebManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool Banned { get; set; }

    public DateTime? BannedUntil { get; set; }

    public DateTime? LastOnline { get; set; }

    public string? AvatarUrl { get; set; }

    public bool IsEmailVerified { get; set; }

    public string? EmailVerificationToken { get; set; }

    public DateTime? EmailVerificationTokenExpiry { get; set; }
}
