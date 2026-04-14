namespace Unalive_WebManagement.DTOs
{
    public class StaffDto
    {
        public int StaffId { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? AvatarUrl { get; set; }
    }

    public class UpdateStaffProfileDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
