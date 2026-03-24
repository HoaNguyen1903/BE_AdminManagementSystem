using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class BannerItemService : IBannerItemService
    {
        private readonly IBannerItemRepository _bannerItemRepository;

        public BannerItemService(IBannerItemRepository bannerItemRepository)
        {
            _bannerItemRepository = bannerItemRepository;
        }

        public async Task<IEnumerable<BannerItemDto>> GetAllBannerItemsAsync()
        {
            var bannerItems = await _bannerItemRepository.GetAllAsync();
            return bannerItems.Select(bi => new BannerItemDto
            {
                BannerId = bi.BannerId,
                ItemId = bi.ItemId,
                RateIncreaseValue = bi.RateIncreaseValue
            });
        }

        public async Task<BannerItemDto?> GetBannerItemByIdAsync(int bannerId, int itemId)
        {
            var bannerItem = await _bannerItemRepository.GetByIdAsync(bannerId, itemId);
            if (bannerItem == null) return null;

            return new BannerItemDto
            {
                BannerId = bannerItem.BannerId,
                ItemId = bannerItem.ItemId,
                RateIncreaseValue = bannerItem.RateIncreaseValue
            };
        }

        public async Task<BannerItemDto> CreateBannerItemAsync(CreateBannerItemDto dto)
        {
            var bannerItem = new BannerItem
            {
                BannerId = dto.BannerId,
                ItemId = dto.ItemId,
                RateIncreaseValue = dto.RateIncreaseValue
            };

            var created = await _bannerItemRepository.AddAsync(bannerItem);

            return new BannerItemDto
            {
                BannerId = created.BannerId,
                ItemId = created.ItemId,
                RateIncreaseValue = created.RateIncreaseValue
            };
        }

        public async Task DeleteBannerItemAsync(int bannerId, int itemId)
        {
            await _bannerItemRepository.DeleteAsync(bannerId, itemId);
        }
    }
}
