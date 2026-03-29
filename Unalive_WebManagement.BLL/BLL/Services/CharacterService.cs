using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterStatRepository _statRepository;
        private readonly ICharacterPvPRepository _pvpRepository;
        private readonly ICharacterPassiveRepository _passiveRepository;
        private readonly ICharacterSkillRepository _skillRepository;
        private readonly ICharacterAttackRepository _attackRepository;
        private readonly IAbilitiesSetRepository _abilitiesRepository;

        public CharacterService(
            ICharacterStatRepository statRepository,
            ICharacterPvPRepository pvpRepository,
            ICharacterPassiveRepository passiveRepository,
            ICharacterSkillRepository skillRepository,
            ICharacterAttackRepository attackRepository,
            IAbilitiesSetRepository abilitiesRepository)
        {
            _statRepository = statRepository;
            _pvpRepository = pvpRepository;
            _passiveRepository = passiveRepository;
            _skillRepository = skillRepository;
            _attackRepository = attackRepository;
            _abilitiesRepository = abilitiesRepository;
        }

        public async Task<IEnumerable<CharacterStatDto>> GetAllCharacterStatsAsync()
        {
            var stats = await _statRepository.GetAllAsync();
            return stats.Select(s => new CharacterStatDto
            {
                CharacterId = s.CharacterId,
                MoveRange = s.MoveRange,
                MaxHealth = s.MaxHealth
            });
        }

        public async Task<CharacterStatDto?> GetCharacterStatByIdAsync(int id)
        {
            var s = await _statRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new CharacterStatDto { CharacterId = s.CharacterId, MoveRange = s.MoveRange, MaxHealth = s.MaxHealth };
        }

        public async Task<IEnumerable<CharacterPvPDto>> GetAllCharacterPvPsAsync()
        {
            var pvps = await _pvpRepository.GetAllAsync();
            return pvps.Select(p => new CharacterPvPDto
            {
                CharacterTacticId = p.CharacterTacticId,
                Name = p.Name,
                Description = p.Description
            });
        }

        public async Task<CharacterPvPDto?> GetCharacterPvPByIdAsync(int id)
        {
            var p = await _pvpRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new CharacterPvPDto { CharacterTacticId = p.CharacterTacticId, Name = p.Name, Description = p.Description };
        }

        public async Task<IEnumerable<CharacterPassiveDto>> GetAllCharacterPassivesAsync()
        {
            var passives = await _passiveRepository.GetAllAsync();
            return passives.Select(p => new CharacterPassiveDto
            {
                CharacterPassiveId = p.CharacterPassiveId,
                Name = p.Name,
                Description = p.Description,
                LockedState = p.LockedState
            });
        }

        public async Task<CharacterPassiveDto?> GetCharacterPassiveByIdAsync(int id)
        {
            var p = await _passiveRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new CharacterPassiveDto { CharacterPassiveId = p.CharacterPassiveId, Name = p.Name, Description = p.Description, LockedState = p.LockedState };
        }

        public async Task<IEnumerable<CharacterSkillDto>> GetAllCharacterSkillsAsync()
        {
            var skills = await _skillRepository.GetAllAsync();
            return skills.Select(s => new CharacterSkillDto
            {
                CharacterSkillId = s.CharacterSkillId,
                Name = s.Name,
                Description = s.Description,
                Damage = s.Damage,
                SP = s.SP,
                YuanPressure = s.YuanPressure,
                CritDmg = s.CritDmg,
                CritRate = s.CritRate,
                LockedState = s.LockedState
            });
        }

        public async Task<CharacterSkillDto?> GetCharacterSkillByIdAsync(int id)
        {
            var s = await _skillRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new CharacterSkillDto
            {
                CharacterSkillId = s.CharacterSkillId,
                Name = s.Name,
                Description = s.Description,
                Damage = s.Damage,
                SP = s.SP,
                YuanPressure = s.YuanPressure,
                CritDmg = s.CritDmg,
                CritRate = s.CritRate,
                LockedState = s.LockedState
            };
        }

        public async Task<IEnumerable<CharacterAttackDto>> GetAllCharacterAttacksAsync()
        {
            var attacks = await _attackRepository.GetAllAsync();
            return attacks.Select(a => new CharacterAttackDto
            {
                CharacterAttackId = a.CharacterAttackId,
                Name = a.Name,
                Description = a.Description,
                Damage = a.Damage,
                AP = a.AP,
                YuanPressure = a.YuanPressure,
                CritDmg = a.CritDmg,
                CritRate = a.CritRate,
                LockedState = a.LockedState
            });
        }

        public async Task<CharacterAttackDto?> GetCharacterAttackByIdAsync(int id)
        {
            var a = await _attackRepository.GetByIdAsync(id);
            if (a == null) return null;
            return new CharacterAttackDto
            {
                CharacterAttackId = a.CharacterAttackId,
                Name = a.Name,
                Description = a.Description,
                Damage = a.Damage,
                AP = a.AP,
                YuanPressure = a.YuanPressure,
                CritDmg = a.CritDmg,
                CritRate = a.CritRate,
                LockedState = a.LockedState
            };
        }

        public async Task<AbilitiesSetDto?> GetAbilitiesSetByTacticIdAsync(int id)
        {
            var a = await _abilitiesRepository.GetByIdAsync(id);
            if (a == null) return null;
            return new AbilitiesSetDto
            {
                CharacterTacticId = a.CharacterTacticId,
                CharacterPassiveId = a.CharacterPassiveId,
                CharacterSkillId = a.CharacterSkillId,
                CharacterAttackId = a.CharacterAttackId
            };
        }
    }
}
