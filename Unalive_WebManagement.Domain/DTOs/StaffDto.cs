namespace Unalive_WebManagement.DTOs
{
    public class StaffDto
    {
        public int StaffId { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
