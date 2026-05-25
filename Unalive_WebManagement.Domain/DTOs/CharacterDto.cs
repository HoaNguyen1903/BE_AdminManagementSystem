namespace Unalive_WebManagement.DTOs
{
    public class CharacterStatDto
    {
        public int CharacterId { get; set; }
        public int MoveRange { get; set; }
        public int MaxHealth { get; set; }
    }

    public class CharacterPvPDto
    {
        public int CharacterTacticId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CharacterPassiveDto
    {
        public int CharacterPassiveId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public short LockedState { get; set; }
    }

    public class CharacterSkillDto
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

    public class CharacterAttackDto
    {
        public int CharacterAttackId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Damage { get; set; }
        public int CritDmg { get; set; }
        public int CritRate { get; set; }
        public CharacterAttackNormalInfo? NormalInfo { get; set; }
        public CharacterAttackYuanInfo? YuanInfo { get; set; }
        public short LockedState { get; set; }
    }

    [Serializable]
    public class CharacterAttackNormalInfo
    {
        public int ActionPointCost = 1;
        public int YuanLiCost = 1;
        public int SkillPointCost = 0;
    }

    [Serializable]
    public class CharacterAttackYuanInfo
    {
        public int ActionPointCost = 1;
        public int YuanLiCost = 1;
        public int SkillPointCost = 0;
    }


    public class AbilitiesSetDto
    {
        public int CharacterTacticId { get; set; }
        public int CharacterPassiveId { get; set; }
        public int CharacterSkillId { get; set; }
        public int CharacterAttackId { get; set; }
    }

    public class UpdateCharacterStatDto
    {
        public int MoveRange { get; set; }
        public int MaxHealth { get; set; }
    }

    public class UpdateCharacterPvPDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateCharacterPassiveDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public short LockedState { get; set; }
    }

    public class UpdateCharacterSkillDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Damage { get; set; }
        public int SP { get; set; }
        public int YuanPressure { get; set; }
        public int CritDmg { get; set; }
        public int CritRate { get; set; }
        public short LockedState { get; set; }
    }

    public class UpdateCharacterAttackDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Damage { get; set; }
        public int CritDmg { get; set; }
        public int CritRate { get; set; }
        public CharacterAttackNormalInfo NormalInfo { get; set; } = null!;
        public CharacterAttackYuanInfo? YuanInfo { get; set; }
        public short LockedState { get; set; }
    }

    public class CreateCharacterStatDto
    {
        public int MoveRange { get; set; }
        public int MaxHealth { get; set; }
    }

    public class CreateCharacterPvPDto
    {
        public int CharacterTacticId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CreateCharacterPassiveDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public short LockedState { get; set; }
    }

    public class CreateCharacterSkillDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Damage { get; set; }
        public int SP { get; set; }
        public int YuanPressure { get; set; }
        public int CritDmg { get; set; }
        public int CritRate { get; set; }
        public short LockedState { get; set; }
    }

    public class CreateCharacterAttackDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Damage { get; set; }
        public int CritDmg { get; set; }
        public int CritRate { get; set; }
        public CharacterAttackNormalInfo NormalInfo { get; set; } = null!;
        public CharacterAttackYuanInfo? YuanInfo { get; set; }
        public short LockedState { get; set; }
    }

    public class CharacterFilterParameters : QueryParameters
    {
        public short? LockedState { get; set; }
    }
}
