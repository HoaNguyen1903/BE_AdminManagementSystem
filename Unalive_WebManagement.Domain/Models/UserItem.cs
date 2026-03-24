using System;
using System.Collections.Generic;

namespace Unalive_WebManagement.Models;

public partial class UserItem
{
    public int UserItemId { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public DateTime ObtainedDate { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
