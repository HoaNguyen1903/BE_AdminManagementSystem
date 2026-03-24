using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
        {
            return Ok(await _staffService.GetAllStaffAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetById(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<StaffDto>> GetByEmail(string email)
        {
            var staff = await _staffService.GetStaffByEmailAsync(email);
            if (staff == null) return NotFound();
            return Ok(staff);
        }
    }
}
