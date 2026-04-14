using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IBlobService _blobService;

        public StaffController(IStaffRepository staffRepository, IBlobService blobService)
        {
            _staffRepository = staffRepository;
            _blobService = blobService;
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (User.IsInRole("User"))
                return Forbid(); // Prevent normal users from updating staff avatars

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Only JPEG, PNG and WebP are allowed");

            if (file.Length > 2 * 1024 * 1024)
                return BadRequest("File size cannot exceed 2MB");

            var staffId = GetAuthenticatedStaffId();
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"avatars/staff/{staffId}/{Guid.NewGuid()}{extension}";

            var url = await _blobService.UploadImageAsync(file, fileName);
            
            var staff = await _staffRepository.GetByIdAsync(staffId);
            if (staff != null)
            {
                staff.AvatarUrl = url;
                await _staffRepository.UpdateAsync(staff);
            }

            return Ok(new { avatarUrl = url });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null) return NotFound();

            return Ok(new 
            {
                staff.StaffId,
                staff.Email,
                staff.Role,
                staff.AvatarUrl
            });
        }

        private int GetAuthenticatedStaffId()
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(nameIdentifier, out var idFromNameIdentifier)) return idFromNameIdentifier;

            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue("sub");
            if (int.TryParse(sub, out var idFromSub)) return idFromSub;

            throw new UnauthorizedAccessException("Staff id claim is missing.");
        }
    }
}