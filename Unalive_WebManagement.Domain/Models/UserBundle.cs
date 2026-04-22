using System;
using System.ComponentModel.DataAnnotations;

namespace Unalive_WebManagement.Models;

public partial class UserBundle
{
    [Key]
    public int UserBundleId { get; set; }

    public int UserId { get; set; }

    public int? SkinAndCharacterBundleId { get; set; }

    public int? GemBundleId { get; set; }

    public int Remaining { get; set; }

    // Add navigation properties to stabilize EF Core's relationship tracking
    public virtual User? User { get; set; }
    public virtual SkinAndCharacterBundle? SkinAndCharacterBundle { get; set; }
    public virtual GemBundle? GemBundle { get; set; }
}
