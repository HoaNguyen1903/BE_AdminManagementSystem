using System;

namespace Unalive_WebManagement.Models;

public partial class UserBundle
{
    public int UserId { get; set; }

    public int? SkinAndCharacterBundleId { get; set; }

    public int? GemBundleId { get; set; }

    public int Remaining { get; set; }
}
