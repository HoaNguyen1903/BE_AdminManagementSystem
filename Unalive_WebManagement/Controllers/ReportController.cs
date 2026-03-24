using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAll()
        {
            return Ok(await _reportService.GetAllReportsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var staffIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (staffIdClaim == null || !int.TryParse(staffIdClaim.Value, out int staffId))
            {
                return Unauthorized("Invalid token");
            }

            var result = await _reportService.ApproveReportAsync(id, staffId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Report approved successfully" });
        }
    }
}
