namespace Unalive_WebManagement.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public short Banned { get; set; }
        public DateTimeOffset? BannedUntil { get; set; } = null!;
        public DateTimeOffset? LastOnline { get; set; }
        public short IsOnline { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }

    public class UpdateUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public short Banned { get; set; }
        public DateTimeOffset? BannedUntil { get; set; }
        public DateTimeOffset? LastOnline { get; set; }
        public short IsOnline { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class UserStatusDto
    {
        public int UserId { get; set; }
        public short IsOnline { get; set; }
        public DateTimeOffset? LastOnline { get; set; }
    }

    public class BanUserRequest
    {
        public string BanReason { get; set; } = null!;
        public DateTimeOffset? BannedUntil { get; set; }
    }

    public class UserItemDto
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ShopOrderId { get; set; }
    }

    public class UserItemWithNameDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class CreateUserItemDto
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ShopOrderId { get; set; }
    }

    public class UpdateUserItemDto
    {
        public int Quantity { get; set; }
        public int ShopOrderId { get; set; }
    }

    public class UserBundleDto
    {
        public int UserId { get; set; }
        public int? SkinAndCharacterBundleId { get; set; }
        public int? GemBundleId { get; set; }
        public int Remaining { get; set; }
    }

    public class CreateUserBundleDto
    {
        public int UserId { get; set; }
        public int? SkinAndCharacterBundleId { get; set; }
        public int? GemBundleId { get; set; }
        public int Remaining { get; set; }
    }

    public class UpdateUserBundleDto
    {
        public int Remaining { get; set; }
    }

    public class UserBanLogDto
    {
        public int UserBanLogId { get; set; }
        public int UserId { get; set; }
        public string BanReason { get; set; } = null!;
        public DateTimeOffset BannedDate { get; set; }
        public DateTimeOffset? BannedUntil { get; set; }
        public int BannedBy { get; set; }
    }

    public class CreateUserBanLogDto
    {
        public int UserId { get; set; }
        public string BanReason { get; set; } = null!;
        public DateTimeOffset? BannedUntil { get; set; }
    }

    public class UpdateUserBanLogDto
    {
        public string BanReason { get; set; } = null!;
        public DateTimeOffset? BannedUntil { get; set; }
    }

    public class UserFilterParameters : QueryParameters
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public short? IsOnline { get; set; }
        public short? IsEmailVerified { get; set; }
    }
}
