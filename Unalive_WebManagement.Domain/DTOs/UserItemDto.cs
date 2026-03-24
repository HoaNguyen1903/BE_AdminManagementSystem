namespace Unalive_WebManagement.DTOs
{
    public class UserItemDto
    {
        public int UserItemId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime ObtainedDate { get; set; }
    }

    public class CreateUserItemDto
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateUserItemDto
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}

