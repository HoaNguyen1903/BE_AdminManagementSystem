namespace Unalive_WebManagement.DTOs
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemType { get; set; } = null!;
    }

    public class CreateItemDto
    {
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemType { get; set; } = null!;
    }

    public class UpdateItemDto
    {
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemType { get; set; } = null!;
    }
}
