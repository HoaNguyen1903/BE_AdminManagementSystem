using System;

namespace Unalive_WebManagement.Models;

public partial class CharacterPassive
{
    public int CharacterPassiveId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public short LockedState { get; set; }
}
