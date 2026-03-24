using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public int Type { get; set; }

    public virtual ICollection<BannerItem> BannerItems { get; set; } = new List<BannerItem>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
