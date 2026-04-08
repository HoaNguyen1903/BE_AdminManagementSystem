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

        [HttpGet("passives")]
        public async Task<ActionResult<IEnumerable<CharacterPassiveDto>>> GetAllPassives([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterPassivesAsync(query));
        }

        [HttpGet("skills")]
        public async Task<ActionResult<IEnumerable<CharacterSkillDto>>> GetAllSkills([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterSkillsAsync(query));
        }

        [HttpGet("attacks")]
        public async Task<ActionResult<IEnumerable<CharacterAttackDto>>> GetAllAttacks([FromQuery] QueryParameters query)
        {
            return Ok(await _characterService.GetAllCharacterAttacksAsync(query));
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
