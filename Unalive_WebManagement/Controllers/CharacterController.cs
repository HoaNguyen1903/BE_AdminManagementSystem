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

        [HttpGet("stats")]
        public async Task<ActionResult<IEnumerable<CharacterStatDto>>> GetAllStats()
        {
            return Ok(await _characterService.GetAllCharacterStatsAsync());
        }

        [HttpGet("stats/{id}")]
        public async Task<ActionResult<CharacterStatDto>> GetStatById(int id)
        {
            var stat = await _characterService.GetCharacterStatByIdAsync(id);
            if (stat == null) return NotFound();
            return Ok(stat);
        }

        [HttpGet("pvp")]
        public async Task<ActionResult<IEnumerable<CharacterPvPDto>>> GetAllPvPs()
        {
            return Ok(await _characterService.GetAllCharacterPvPsAsync());
        }

        [HttpGet("pvp/{id}")]
        public async Task<ActionResult<CharacterPvPDto>> GetPvPById(int id)
        {
            var pvp = await _characterService.GetCharacterPvPByIdAsync(id);
            if (pvp == null) return NotFound();
            return Ok(pvp);
        }

        [HttpGet("passives")]
        public async Task<ActionResult<IEnumerable<CharacterPassiveDto>>> GetAllPassives()
        {
            return Ok(await _characterService.GetAllCharacterPassivesAsync());
        }

        [HttpGet("skills")]
        public async Task<ActionResult<IEnumerable<CharacterSkillDto>>> GetAllSkills()
        {
            return Ok(await _characterService.GetAllCharacterSkillsAsync());
        }

        [HttpGet("attacks")]
        public async Task<ActionResult<IEnumerable<CharacterAttackDto>>> GetAllAttacks()
        {
            return Ok(await _characterService.GetAllCharacterAttacksAsync());
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
