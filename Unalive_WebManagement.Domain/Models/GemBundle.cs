using System;

namespace Unalive_WebManagement.Models;

public partial class GemBundle
{
    public int GemBundleId { get; set; }

    public string BundleName { get; set; } = null!;

    public float BundlePrice { get; set; }
}
