using System;

namespace Unalive_WebManagement.Models;

public partial class CharacterAttack
{
    public int CharacterAttackId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Damage { get; set; }

    public int CritDmg { get; set; }

    public int CritRate { get; set; }
    public string? NormalInfo { get; set; }
    public string? YuanInfo { get; set; }
    public short LockedState { get; set; }
}
