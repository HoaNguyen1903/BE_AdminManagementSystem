namespace Unalive_WebManagement.DTOs
{
    public class BannerItemDto
    {
        public int BannerId { get; set; }
        public int ItemId { get; set; }
        public int RateIncreaseValue { get; set; }
    }

    public class CreateBannerItemDto
    {
        public int BannerId { get; set; }
        public int ItemId { get; set; }
        public int RateIncreaseValue { get; set; }
    }
}
