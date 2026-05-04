using System;

namespace Unalive_WebManagement.Models;

public partial class SkinAndCharacterBundle
{
    public int SkinAndCharacterBundleId { get; set; }

    public string BundleName { get; set; } = null!;

    public float BundlePrice { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public string? imageUrl { get; set; }
}
