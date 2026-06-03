using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [AllowAnonymous]
        [HttpGet("stats")]
        public async Task<ActionResult<IEnumerable<CharacterStatDto>>> GetAllStats([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterStatsAsync(query));
        }

        [HttpGet("stats/{id}")]
        public async Task<ActionResult<CharacterStatDto>> GetStatById(int id)
        {
            var stat = await _characterService.GetCharacterStatByIdAsync(id);
            if (stat == null) return NotFound();
            return Ok(stat);
        }

        [HttpPost("stats")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<CharacterStatDto>> CreateStat([FromBody] CreateCharacterStatDto dto)
        {
            try
            {
                var stat = await _characterService.CreateCharacterStatAsync(dto);
                return CreatedAtAction(nameof(GetStatById), new { id = stat.CharacterId }, stat);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("stats/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStat(int id, [FromBody] UpdateCharacterStatDto dto)
        {
            try
            {
                await _characterService.UpdateCharacterStatAsync(id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("pvp")]
        public async Task<ActionResult<IEnumerable<CharacterPvPDto>>> GetAllPvPs([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterPvPsAsync(query));
        }

        [HttpGet("pvp/{id}")]
        public async Task<ActionResult<CharacterPvPDto>> GetPvPById(int id)
        {
            var pvp = await _characterService.GetCharacterPvPByIdAsync(id);
            if (pvp == null) return NotFound();
            return Ok(pvp);
        }

        [HttpPost("pvp")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<CharacterPvPDto>> CreatePvP([FromBody] CreateCharacterPvPDto dto)
        {
            try
            {
                var pvp = await _characterService.CreateCharacterPvPAsync(dto);
                return CreatedAtAction(nameof(GetPvPById), new { id = pvp.CharacterTacticId }, pvp);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("pvp/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdatePvP(int id, [FromBody] UpdateCharacterPvPDto dto)
        {
            try
            {
                await _characterService.UpdateCharacterPvPAsync(id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [AllowAnonymous]
        [HttpGet("passives")]
        public async Task<ActionResult<IEnumerable<CharacterPassiveDto>>> GetAllPassives([FromQuery] CharacterFilterParameters query)
        {
            return Ok(await _characterService.GetAllCharacterPassivesAsync(query));
        }

        [HttpGet("passives/{id}")]
        public async Task<ActionResult<CharacterPassiveDto>> GetPassiveById(int id)
        {
            var passive = await _characterService.GetCharacterPassiveByIdAsync(id);
            if (passive == null) return NotFound();
            return Ok(passive);
        }

        [HttpPost("passives")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<CharacterPassiveDto>> CreatePassive([FromBody] CreateCharacterPassiveDto dto)
        {
            try
            {
                var passive = await _characterService.CreateCharacterPassiveAsync(dto);
                return CreatedAtAction(nameof(GetPassiveById), new { id = passive.CharacterPassiveId }, passive);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("passives/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdatePassive(int id, [FromBody] UpdateCharacterPassiveDto dto)
        {
            try
            {
                await _characterService.UpdateCharacterPassiveAsync(id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("skills")]
        public async Task<ActionResult<IEnumerable<CharacterSkillDto>>> GetAllSkills([FromQuery] CharacterFilterParameters query)
        {
            return Ok(await _characterService.GetAllCharacterSkillsAsync(query));
        }

        [HttpGet("skills/{id}")]
        public async Task<ActionResult<CharacterSkillDto>> GetSkillById(int id)
        {
            var skill = await _characterService.GetCharacterSkillByIdAsync(id);
            if (skill == null) return NotFound();
            return Ok(skill);
        }

        [HttpPost("skills")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<CharacterSkillDto>> CreateSkill([FromBody] CreateCharacterSkillDto dto)
        {
            try
            {
                var skill = await _characterService.CreateCharacterSkillAsync(dto);
                return CreatedAtAction(nameof(GetSkillById), new { id = skill.CharacterSkillId }, skill);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("skills/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] UpdateCharacterSkillDto dto)
        {
            try
            {
                await _characterService.UpdateCharacterSkillAsync(id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("attacks")]
        public async Task<ActionResult<IEnumerable<CharacterAttackDto>>> GetAllAttacks([FromQuery] CharacterFilterParameters query)
        {
            return Ok(await _characterService.GetAllCharacterAttacksAsync(query));
        }

        [HttpGet("attacks/{id}")]
        public async Task<ActionResult<CharacterAttackDto>> GetAttackById(int id)
        {
            var attack = await _characterService.GetCharacterAttackByIdAsync(id);
            if (attack == null) return NotFound();
            return Ok(attack);
        }

        [HttpPost("attacks")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<CharacterAttackDto>> CreateAttack([FromBody] CreateCharacterAttackDto dto)
        {
            try
            {
                var attack = await _characterService.CreateCharacterAttackAsync(dto);
                return CreatedAtAction(nameof(GetAttackById), new { id = attack.CharacterAttackId }, attack);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("attacks/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateAttack(int id, [FromBody] UpdateCharacterAttackDto dto)
        {
            try
            {
                await _characterService.UpdateCharacterAttackAsync(id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("abilities/{tacticId}")]
        public async Task<ActionResult<AbilitiesSetDto>> GetAbilitiesSet(int tacticId)
        {
            var set = await _characterService.GetAbilitiesSetByTacticIdAsync(tacticId);
            if (set == null) return NotFound();
            return Ok(set);
        }
    }
}
