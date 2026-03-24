namespace Unalive_WebManagement.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime? Banned { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime? Banned { get; set; }
    }

    public class UpdateUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime? Banned { get; set; }
    }
}

