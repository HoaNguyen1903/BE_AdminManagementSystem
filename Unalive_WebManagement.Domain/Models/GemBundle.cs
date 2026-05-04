using System;
using System.ComponentModel.DataAnnotations;

namespace Unalive_WebManagement.Models;

public partial class GemBundle
{
    public int GemBundleId { get; set; }

    public string BundleName { get; set; } = null!;

    [Required]
    public float BundlePrice { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public string? imageUrl { get; set; }
}
