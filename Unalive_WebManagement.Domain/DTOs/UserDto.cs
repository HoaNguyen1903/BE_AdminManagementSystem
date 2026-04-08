namespace Unalive_WebManagement.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool Banned { get; set; }
    }

    public class UserItemDto
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ShopOrderId { get; set; }
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
}
