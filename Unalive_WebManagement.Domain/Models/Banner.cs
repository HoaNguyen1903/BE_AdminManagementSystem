using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class Banner
{
    public int BannerId { get; set; }

    public string BannerImage { get; set; } = null!;

    //Nhớ thêm revenue vào

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<BannerItem> BannerItems { get; set; } = new List<BannerItem>();
}
