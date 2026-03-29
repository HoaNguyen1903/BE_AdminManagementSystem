using System;

namespace Unalive_WebManagement.Models;

public partial class CharacterAttack
{
    public int CharacterAttackId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Damage { get; set; }

    public int AP { get; set; }

    public int YuanPressure { get; set; }

    public int CritDmg { get; set; }

    public int CritRate { get; set; }

    public bool LockedState { get; set; }
}
