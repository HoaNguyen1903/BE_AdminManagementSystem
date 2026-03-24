namespace Unalive_WebManagement.DTOs
{
    public class BannerDto
    {
        public int BannerId { get; set; }
        public string BannerImage { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }

    public class CreateBannerDto
    {
        public string BannerImage { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }

    public class UpdateBannerDto
    {
        public string BannerImage { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}

