using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
        {
            var staffList = await _staffRepository.GetAllAsync();
            return staffList.Select(s => new StaffDto
            {
                StaffId = s.StaffId,
                Email = s.Email,
                FullName = s.FullName,
                Role = s.Role
            });
        }

        public async Task<StaffDto?> GetStaffByIdAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null) return null;

            return new StaffDto
            {
                StaffId = staff.StaffId,
                Email = staff.Email,
                FullName = staff.FullName,
                Role = staff.Role
            };
        }

        public async Task<StaffDto?> GetStaffByEmailAsync(string email)
        {
            var staff = await _staffRepository.GetByEmailAsync(email);
            if (staff == null) return null;

            return new StaffDto
            {
                StaffId = staff.StaffId,
                Email = staff.Email,
                FullName = staff.FullName,
                Role = staff.Role
            };
        }
    }
}
