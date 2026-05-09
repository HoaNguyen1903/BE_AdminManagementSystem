using System;

namespace Unalive_WebManagement.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public string ItemType { get; set; } = null!;
    public string? ItemImageUrl { get; set; }

    public enum StatusEnum
    {
        Available,
        Unavailable,
        Discontinued
    }

    public StatusEnum Status { get; set; } = StatusEnum.Available;
}
