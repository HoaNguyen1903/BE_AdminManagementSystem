using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class BannerItem
{
    public int BannerId { get; set; }

    public int ItemId { get; set; }

    public int RateIncreaseValue { get; set; }

    public virtual Banner Banner { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
