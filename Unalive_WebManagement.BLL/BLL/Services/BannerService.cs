using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IEnumerable<BannerDto>> GetAllBannersAsync()
        {
            var banners = await _bannerRepository.GetAllAsync();
            return banners.Select(b => new BannerDto
            {
                BannerId = b.BannerId,
                BannerImage = b.BannerImage,
                StartDate = b.StartDate,
                EndDate = b.EndDate
            });
        }

        public async Task<BannerDto?> GetBannerByIdAsync(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null) return null;

            return new BannerDto
            {
                BannerId = banner.BannerId,
                BannerImage = banner.BannerImage,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate
            };
        }

        public async Task<BannerDto> CreateBannerAsync(CreateBannerDto dto)
        {
            var banner = new Banner
            {
                BannerImage = dto.BannerImage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            var created = await _bannerRepository.AddAsync(banner);

            return new BannerDto
            {
                BannerId = created.BannerId,
                BannerImage = created.BannerImage,
                StartDate = created.StartDate,
                EndDate = created.EndDate
            };
        }

        public async Task UpdateBannerAsync(int id, UpdateBannerDto dto)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null) throw new KeyNotFoundException($"Banner with ID {id} not found");

            banner.BannerImage = dto.BannerImage;
            banner.StartDate = dto.StartDate;
            banner.EndDate = dto.EndDate;

            await _bannerRepository.UpdateAsync(banner);
        }

        public async Task DeleteBannerAsync(int id)
        {
            await _bannerRepository.DeleteAsync(id);
        }
    }
}

