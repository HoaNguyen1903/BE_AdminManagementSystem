using System;

namespace Unalive_WebManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public short Banned { get; set; }

    public DateTimeOffset? BannedUntil { get; set; }
    public short IsOnline { get; set; }
    public DateTimeOffset? LastOnline { get; set; }

    public string? AvatarUrl { get; set; }

    public short IsEmailVerified { get; set; }

    public string? EmailVerificationToken { get; set; }

    public DateTimeOffset? EmailVerificationTokenExpiry { get; set; }

    public long RankPoint { get; set; }
}
