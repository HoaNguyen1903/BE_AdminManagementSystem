using System;

namespace Unalive_WebManagement.Models;

public partial class CharacterSkill
{
    public int CharacterSkillId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Damage { get; set; }

    public int SP { get; set; }

    public int YuanPressure { get; set; }

    public int CritDmg { get; set; }

    public int CritRate { get; set; }

    public short LockedState { get; set; }
}
