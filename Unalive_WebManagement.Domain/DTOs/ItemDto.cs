namespace Unalive_WebManagement.DTOs
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemType { get; set; } = null!;
        public string? ItemImageUrl { get; set; }
        public string? Status { get; set; }
    }

    public class CreateItemDto
    {
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemType { get; set; } = null!;
        public string? ItemImageUrl { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateItemDto
    {
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemType { get; set; } = null!;
        public string? ItemImageUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ItemFilterParameters : QueryParameters
    {
        public string? Status { get; set; }
        public string? ItemType { get; set; }
    }

    public class AssociatedBundlesDto
    {
        public IEnumerable<SkinAndCharacterBundleDto> SkinAndCharacterBundles { get; set; } = new List<SkinAndCharacterBundleDto>();
        public IEnumerable<GemBundleDto> GemBundles { get; set; } = new List<GemBundleDto>();
    }
}