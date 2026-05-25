using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;
using Newtonsoft.Json;

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

        public async Task<IEnumerable<CharacterStatDto>> GetAllCharacterStatsAsync(QueryParameters query)
        {
            var stats = await _statRepository.GetAllAsync();
            var dtos = stats.Select(s => new CharacterStatDto
            {
                CharacterId = s.CharacterId,
                MoveRange = s.MoveRange,
                MaxHealth = s.MaxHealth
            });
            return dtos.ApplyQuery(query, (s, search) => s.CharacterId.ToString().Contains(search));
        }

        public async Task<CharacterStatDto?> GetCharacterStatByIdAsync(int id)
        {
            var s = await _statRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new CharacterStatDto { CharacterId = s.CharacterId, MoveRange = s.MoveRange, MaxHealth = s.MaxHealth };
        }

        public async Task<CharacterStatDto> CreateCharacterStatAsync(CreateCharacterStatDto dto)
        {
            if (dto.MoveRange < 0) throw new ArgumentException("Move Range cannot be negative.");
            if (dto.MaxHealth <= 0) throw new ArgumentException("Max Health must be greater than zero.");

            var s = new CharacterStat
            {
                MoveRange = dto.MoveRange,
                MaxHealth = dto.MaxHealth
            };
            await _statRepository.AddAsync(s);
            return new CharacterStatDto { CharacterId = s.CharacterId, MoveRange = s.MoveRange, MaxHealth = s.MaxHealth };
        }

        public async Task UpdateCharacterStatAsync(int id, UpdateCharacterStatDto dto)
        {
            if (dto.MoveRange < 0) throw new ArgumentException("Move Range cannot be negative.");
            if (dto.MaxHealth <= 0) throw new ArgumentException("Max Health must be greater than zero.");

            var s = await _statRepository.GetByIdAsync(id);
            if (s == null) throw new KeyNotFoundException();
            s.MoveRange = dto.MoveRange;
            s.MaxHealth = dto.MaxHealth;
            await _statRepository.UpdateAsync(s);
        }

        public async Task<IEnumerable<CharacterPvPDto>> GetAllCharacterPvPsAsync(QueryParameters query)
        {
            var pvps = await _pvpRepository.GetAllAsync();
            var dtos = pvps.Select(p => new CharacterPvPDto
            {
                CharacterTacticId = p.CharacterTacticId,
                Name = p.Name,
                Description = p.Description
            });
            return dtos.ApplyQuery(query, (p, search) =>
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<CharacterPvPDto?> GetCharacterPvPByIdAsync(int id)
        {
            var p = await _pvpRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new CharacterPvPDto { CharacterTacticId = p.CharacterTacticId, Name = p.Name, Description = p.Description };
        }

        public async Task<CharacterPvPDto> CreateCharacterPvPAsync(CreateCharacterPvPDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");

            var stat = await _statRepository.GetByIdAsync(dto.CharacterTacticId);
            if (stat == null) throw new KeyNotFoundException($"Character with ID {dto.CharacterTacticId} does not exist. Stats must be created first.");

            var existing = await _pvpRepository.GetByIdAsync(dto.CharacterTacticId);
            if (existing != null) throw new InvalidOperationException($"Character PvP with Tactic ID {dto.CharacterTacticId} already exists.");

            var p = new CharacterPvP
            {
                CharacterTacticId = dto.CharacterTacticId,
                Name = dto.Name,
                Description = dto.Description
            };
            await _pvpRepository.AddAsync(p);
            return new CharacterPvPDto { CharacterTacticId = p.CharacterTacticId, Name = p.Name, Description = p.Description };
        }

        public async Task UpdateCharacterPvPAsync(int id, UpdateCharacterPvPDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");

            var p = await _pvpRepository.GetByIdAsync(id);
            if (p == null) throw new KeyNotFoundException();
            p.Name = dto.Name;
            p.Description = dto.Description;
            await _pvpRepository.UpdateAsync(p);
        }

        public async Task<IEnumerable<CharacterPassiveDto>> GetAllCharacterPassivesAsync(CharacterFilterParameters query)
        {
            var passives = await _passiveRepository.GetAllAsync();
            var q = passives.AsQueryable();

            if (query.LockedState.HasValue)
            {
                q = q.Where(p => p.LockedState == query.LockedState.Value);
            }

            var dtos = q.Select(p => new CharacterPassiveDto
            {
                CharacterPassiveId = p.CharacterPassiveId,
                Name = p.Name,
                Description = p.Description,
                LockedState = p.LockedState
            });
            return dtos.ApplyQuery(query, (p, search) =>
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<CharacterPassiveDto?> GetCharacterPassiveByIdAsync(int id)
        {
            var p = await _passiveRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new CharacterPassiveDto { CharacterPassiveId = p.CharacterPassiveId, Name = p.Name, Description = p.Description, LockedState = p.LockedState };
        }

        public async Task<CharacterPassiveDto> CreateCharacterPassiveAsync(CreateCharacterPassiveDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");

            // Backend Guard: Prevent duplicate passives by name
            var passives = await _passiveRepository.GetAllAsync();
            if (passives.Any(p => p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A character passive with the name '{dto.Name}' already exists.");
            }

            var p = new CharacterPassive
            {
                Name = dto.Name,
                Description = dto.Description,
                LockedState = dto.LockedState
            };
            await _passiveRepository.AddAsync(p);
            return new CharacterPassiveDto { CharacterPassiveId = p.CharacterPassiveId, Name = p.Name, Description = p.Description, LockedState = p.LockedState };
        }

        public async Task UpdateCharacterPassiveAsync(int id, UpdateCharacterPassiveDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");

            var p = await _passiveRepository.GetByIdAsync(id);
            if (p == null) throw new KeyNotFoundException();
            p.Name = dto.Name;
            p.Description = dto.Description;
            p.LockedState = dto.LockedState;
            await _passiveRepository.UpdateAsync(p);
        }

        public async Task<IEnumerable<CharacterSkillDto>> GetAllCharacterSkillsAsync(CharacterFilterParameters query)
        {
            var skills = await _skillRepository.GetAllAsync();
            var q = skills.AsQueryable();

            if (query.LockedState.HasValue)
            {
                q = q.Where(s => s.LockedState == query.LockedState.Value);
            }

            var dtos = q.Select(s => new CharacterSkillDto
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
            return dtos.ApplyQuery(query, (s, search) =>
                s.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
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

        public async Task<CharacterSkillDto> CreateCharacterSkillAsync(CreateCharacterSkillDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");

            // Backend Guard: Prevent duplicate skills by name
            var skills = await _skillRepository.GetAllAsync();
            if (skills.Any(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A character skill with the name '{dto.Name}' already exists.");
            }

            if (dto.Damage < 0) throw new ArgumentException("Damage cannot be negative.");
            if (dto.SP < 0) throw new ArgumentException("SP cannot be negative.");
            if (dto.YuanPressure < 0) throw new ArgumentException("Yuan Pressure cannot be negative.");
            if (dto.CritDmg < 0) throw new ArgumentException("Crit Dmg cannot be negative.");
            if (dto.CritRate < 0) throw new ArgumentException("Crit Rate cannot be negative.");

            var s = new CharacterSkill
            {
                Name = dto.Name,
                Description = dto.Description,
                Damage = dto.Damage,
                SP = dto.SP,
                YuanPressure = dto.YuanPressure,
                CritDmg = dto.CritDmg,
                CritRate = dto.CritRate,
                LockedState = dto.LockedState
            };
            await _skillRepository.AddAsync(s);
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

        public async Task UpdateCharacterSkillAsync(int id, UpdateCharacterSkillDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");
            if (dto.Damage < 0) throw new ArgumentException("Damage cannot be negative.");
            if (dto.SP < 0) throw new ArgumentException("SP cannot be negative.");
            if (dto.YuanPressure < 0) throw new ArgumentException("Yuan Pressure cannot be negative.");
            if (dto.CritDmg < 0) throw new ArgumentException("Crit Dmg cannot be negative.");
            if (dto.CritRate < 0) throw new ArgumentException("Crit Rate cannot be negative.");

            var s = await _skillRepository.GetByIdAsync(id);
            if (s == null) throw new KeyNotFoundException();
            s.Name = dto.Name;
            s.Description = dto.Description;
            s.Damage = dto.Damage;
            s.SP = dto.SP;
            s.YuanPressure = dto.YuanPressure;
            s.CritDmg = dto.CritDmg;
            s.CritRate = dto.CritRate;
            s.LockedState = dto.LockedState;
            await _skillRepository.UpdateAsync(s);
        }

        public async Task<IEnumerable<CharacterAttackDto>> GetAllCharacterAttacksAsync(CharacterFilterParameters query)
        {
            var attacks = await _attackRepository.GetAllAsync();
            var q = attacks.AsQueryable();

            if (query.LockedState.HasValue)
            {
                q = q.Where(a => a.LockedState == query.LockedState.Value);
            }

            var dtos = q.Select(a => new CharacterAttackDto
            {
                CharacterAttackId = a.CharacterAttackId,
                Name = a.Name,
                Description = a.Description,
                Damage = a.Damage,
                CritDmg = a.CritDmg,
                CritRate = a.CritRate,
                NormalInfo = JsonConvert.DeserializeObject<CharacterAttackNormalInfo>(a.NormalInfo ?? ""),
                YuanInfo = JsonConvert.DeserializeObject<CharacterAttackYuanInfo>(a.YuanInfo ?? ""),
                LockedState = a.LockedState
            });
            return dtos.ApplyQuery(query, (a, search) =>
                a.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
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
                CritDmg = a.CritDmg,
                CritRate = a.CritRate,
                NormalInfo = JsonConvert.DeserializeObject<CharacterAttackNormalInfo>(a.NormalInfo ?? ""),
                YuanInfo = JsonConvert.DeserializeObject<CharacterAttackYuanInfo>(a.YuanInfo ?? ""),
                LockedState = a.LockedState
            };
        }

        public async Task<CharacterAttackDto> CreateCharacterAttackAsync(CreateCharacterAttackDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");

            // Backend Guard: Prevent duplicate attacks by name
            var attacks = await _attackRepository.GetAllAsync();
            if (attacks.Any(a => a.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A character attack with the name '{dto.Name}' already exists.");
            }

            if (dto.Damage < 0) throw new ArgumentException("Damage cannot be negative.");
            if (dto.NormalInfo == null) throw new ArgumentException("Normal cannot be null.");
            if (dto.NormalInfo.ActionPointCost < 0) throw new ArgumentException("Action Point Cost cannot be null.");
            if (dto.NormalInfo.SkillPointCost < 0) throw new ArgumentException("Skill Point Cost cannot be null.");
            if (dto.CritDmg < 0) throw new ArgumentException("Crit Dmg cannot be negative.");
            if (dto.CritRate < 0) throw new ArgumentException("Crit Rate cannot be negative.");

            var a = new CharacterAttack
            {
                Name = dto.Name,
                Description = dto.Description,
                Damage = dto.Damage,
                CritDmg = dto.CritDmg,
                CritRate = dto.CritRate,
                NormalInfo = JsonConvert.SerializeObject(dto.NormalInfo),
                YuanInfo = JsonConvert.SerializeObject(dto.YuanInfo),
                LockedState = dto.LockedState
            };
            await _attackRepository.AddAsync(a);
            return new CharacterAttackDto
            {
                CharacterAttackId = a.CharacterAttackId,
                Name = a.Name,
                Description = a.Description,
                Damage = a.Damage,
                CritDmg = a.CritDmg,
                CritRate = a.CritRate,
                NormalInfo = JsonConvert.DeserializeObject<CharacterAttackNormalInfo>(a.NormalInfo),
                YuanInfo = JsonConvert.DeserializeObject<CharacterAttackYuanInfo>(a.YuanInfo),
                LockedState = a.LockedState
            };
        }

        public async Task UpdateCharacterAttackAsync(int id, UpdateCharacterAttackDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");
            if (dto.Damage < 0) throw new ArgumentException("Damage cannot be negative.");
            if (dto.NormalInfo == null) throw new ArgumentException("Normal cannot be null.");
            if (dto.NormalInfo.ActionPointCost < 0) throw new ArgumentException("Action Point Cost cannot be null.");
            if (dto.NormalInfo.SkillPointCost < 0) throw new ArgumentException("Skill Point Cost cannot be null.");
            if (dto.CritDmg < 0) throw new ArgumentException("Crit Dmg cannot be negative.");
            if (dto.CritRate < 0) throw new ArgumentException("Crit Rate cannot be negative.");

            var a = await _attackRepository.GetByIdAsync(id);
            if (a == null) throw new KeyNotFoundException();
            a.Name = dto.Name;
            a.Description = dto.Description;
            a.Damage = dto.Damage;
            a.CritDmg = dto.CritDmg;
            a.CritRate = dto.CritRate;
            a.NormalInfo = JsonConvert.SerializeObject(dto.NormalInfo);
            a.YuanInfo = JsonConvert.SerializeObject(dto.YuanInfo);
            a.LockedState = dto.LockedState;
            await _attackRepository.UpdateAsync(a);
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
