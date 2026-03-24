namespace Unalive_WebManagement.DTOs
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int Type { get; set; }
    }

    public class CreateItemDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int Type { get; set; }
    }

    public class UpdateItemDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int Type { get; set; }
    }
}

