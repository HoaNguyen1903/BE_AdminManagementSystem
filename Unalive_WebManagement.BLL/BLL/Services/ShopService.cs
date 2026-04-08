using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;

namespace Unalive_WebManagement.BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly IGemBundleRepository _gemBundleRepository;
        private readonly ISkinAndCharacterBundleRepository _skinAndCharacterBundleRepository;
        private readonly IShopOrderRepository _shopOrderRepository;
        private readonly IShopOrderDetailRepository _shopOrderDetailRepository;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;
        private readonly IBundleItemRepository _bundleItemRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly INotificationRepository _notificationRepository;

        public ShopService(
            IGemBundleRepository gemBundleRepository,
            ISkinAndCharacterBundleRepository skinAndCharacterBundleRepository,
            IShopOrderRepository shopOrderRepository,
            IShopOrderDetailRepository shopOrderDetailRepository,
            ITopUpHistoryRepository topUpHistoryRepository,
            IBundleItemRepository bundleItemRepository,
            IAnnouncementRepository announcementRepository,
            INotificationRepository notificationRepository)
        {
            _gemBundleRepository = gemBundleRepository;
            _skinAndCharacterBundleRepository = skinAndCharacterBundleRepository;
            _shopOrderRepository = shopOrderRepository;
            _shopOrderDetailRepository = shopOrderDetailRepository;
            _topUpHistoryRepository = topUpHistoryRepository;
            _bundleItemRepository = bundleItemRepository;
            _announcementRepository = announcementRepository;
            _notificationRepository = notificationRepository;
        }

        // GemBundle CRUD
        public async Task<IEnumerable<GemBundleDto>> GetAllGemBundlesAsync(QueryParameters query)
        {
            var bundles = await _gemBundleRepository.GetAllAsync();
            var dtos = bundles.Select(b => new GemBundleDto
            {
                GemBundleId = b.GemBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice
            });
            return dtos.ApplyQuery(query, (b, search) => b.BundleName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<GemBundleDto?> GetGemBundleByIdAsync(int id)
        {
            var b = await _gemBundleRepository.GetByIdAsync(id);
            if (b == null) return null;
            return new GemBundleDto { GemBundleId = b.GemBundleId, BundleName = b.BundleName, BundlePrice = b.BundlePrice };
        }

        public async Task<GemBundleDto> CreateGemBundleAsync(CreateGemBundleDto dto, int staffId)
        {
            var bundle = new GemBundle { BundleName = dto.BundleName, BundlePrice = dto.BundlePrice };
            var created = await _gemBundleRepository.AddAsync(bundle);

            // Create automatic announcement
            var announcement = new Announcement
            {
                Title = $"[{created.BundleName}] added to the game",
                Content = "",
                Type = "General",
                Status = "Drafted",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                CreatedBy = staffId,
                CreatedAt = DateTime.UtcNow
            };
            await _announcementRepository.AddAsync(announcement);

            // Create notification to staff
            var notification = new Notification
            {
                NotificationMessage = "There's an announcement waiting to be edited or published.",
                ReceiverId = staffId,
                Read = false
            };
            await _notificationRepository.AddAsync(notification);

            return new GemBundleDto { GemBundleId = created.GemBundleId, BundleName = created.BundleName, BundlePrice = created.BundlePrice };
        }

        public async Task UpdateGemBundleAsync(int id, UpdateGemBundleDto dto)
        {
            var bundle = await _gemBundleRepository.GetByIdAsync(id);
            if (bundle == null) throw new KeyNotFoundException();
            bundle.BundleName = dto.BundleName;
            bundle.BundlePrice = dto.BundlePrice;
            await _gemBundleRepository.UpdateAsync(bundle);
        }

        public async Task DeleteGemBundleAsync(int id) => await _gemBundleRepository.DeleteAsync(id);

        // SkinAndCharacterBundle CRUD
        public async Task<IEnumerable<SkinAndCharacterBundleDto>> GetAllSkinAndCharacterBundlesAsync(QueryParameters query)
        {
            var bundles = await _skinAndCharacterBundleRepository.GetAllAsync();
            var dtos = bundles.Select(b => new SkinAndCharacterBundleDto
            {
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice
            });
            return dtos.ApplyQuery(query, (b, search) => b.BundleName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<SkinAndCharacterBundleDto?> GetSkinAndCharacterBundleByIdAsync(int id)
        {
            var b = await _skinAndCharacterBundleRepository.GetByIdAsync(id);
            if (b == null) return null;
            return new SkinAndCharacterBundleDto { SkinAndCharacterBundleId = b.SkinAndCharacterBundleId, BundleName = b.BundleName, BundlePrice = b.BundlePrice };
        }

        public async Task<SkinAndCharacterBundleDto> CreateSkinAndCharacterBundleAsync(CreateSkinAndCharacterBundleDto dto, int staffId)
        {
            var bundle = new SkinAndCharacterBundle { BundleName = dto.BundleName, BundlePrice = dto.BundlePrice };
            var created = await _skinAndCharacterBundleRepository.AddAsync(bundle);

            // Create automatic announcement
            var announcement = new Announcement
            {
                Title = $"[{created.BundleName}] added to the game",
                Content = "",
                Type = "General",
                Status = "Drafted",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                CreatedBy = staffId,
                CreatedAt = DateTime.UtcNow
            };
            await _announcementRepository.AddAsync(announcement);

            // Create notification to staff
            var notification = new Notification
            {
                NotificationMessage = "There's an announcement waiting to be edited or published.",
                ReceiverId = staffId,
                Read = false
            };
            await _notificationRepository.AddAsync(notification);

            return new SkinAndCharacterBundleDto { SkinAndCharacterBundleId = created.SkinAndCharacterBundleId, BundleName = created.BundleName, BundlePrice = created.BundlePrice };
        }

        public async Task UpdateSkinAndCharacterBundleAsync(int id, UpdateSkinAndCharacterBundleDto dto)
        {
            var bundle = await _skinAndCharacterBundleRepository.GetByIdAsync(id);
            if (bundle == null) throw new KeyNotFoundException();
            bundle.BundleName = dto.BundleName;
            bundle.BundlePrice = dto.BundlePrice;
            await _skinAndCharacterBundleRepository.UpdateAsync(bundle);
        }

        public async Task DeleteSkinAndCharacterBundleAsync(int id) => await _skinAndCharacterBundleRepository.DeleteAsync(id);

        // BundleItem CRUD
        public async Task<IEnumerable<BundleItemDto>> GetAllBundleItemsAsync(QueryParameters query)
        {
            var items = await _bundleItemRepository.GetAllAsync();
            var dtos = items.Select(i => new BundleItemDto
            {
                SkinAndCharacterBundleId = i.SkinAndCharacterBundleId,
                ItemId = i.ItemId,
                Quantity = i.Quantity
            });
            return dtos.ApplyQuery(query, (i, search) => i.SkinAndCharacterBundleId.ToString().Contains(search) || i.ItemId.ToString().Contains(search));
        }

        public async Task<IEnumerable<BundleItemDto>> GetItemsByBundleIdAsync(int bundleId)
        {
            var items = await _bundleItemRepository.GetAllAsync();
            return items.Where(i => i.SkinAndCharacterBundleId == bundleId).Select(i => new BundleItemDto
            {
                SkinAndCharacterBundleId = i.SkinAndCharacterBundleId,
                ItemId = i.ItemId,
                Quantity = i.Quantity
            });
        }

        public async Task<BundleItemDto> CreateBundleItemAsync(CreateBundleItemDto dto)
        {
            var item = new BundleItem { SkinAndCharacterBundleId = dto.SkinAndCharacterBundleId, ItemId = dto.ItemId, Quantity = dto.Quantity };
            var created = await _bundleItemRepository.AddAsync(item);
            return new BundleItemDto { SkinAndCharacterBundleId = created.SkinAndCharacterBundleId, ItemId = created.ItemId, Quantity = created.Quantity };
        }

        public async Task UpdateBundleItemAsync(int bundleId, int itemId, UpdateBundleItemDto dto)
        {
            var item = await _bundleItemRepository.GetByIdAsync(bundleId, itemId);
            if (item == null) throw new KeyNotFoundException();
            item.Quantity = dto.Quantity;
            await _bundleItemRepository.UpdateAsync(item);
        }

        public async Task DeleteBundleItemAsync(int bundleId, int itemId)
        {
            await _bundleItemRepository.DeleteAsync(bundleId, itemId);
        }

        // ShopOrder & Details & TopUp (Read Only)
        public async Task<IEnumerable<ShopOrderDto>> GetAllShopOrdersAsync(QueryParameters query)
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            var dtos = orders.Select(o => new ShopOrderDto
            {
                ShopOrderId = o.ShopOrderId,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate
            });
            return dtos.ApplyQuery(query, (o, search) => o.UserId.ToString().Contains(search));
        }

        public async Task<IEnumerable<ShopOrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId, QueryParameters query)
        {
            var details = await _shopOrderDetailRepository.GetAllAsync();
            var dtos = details.Where(d => d.ShopOrderId == orderId).Select(d => new ShopOrderDetailDto
            {
                ShopOrderDetailId = d.ShopOrderDetailId,
                ShopOrderId = d.ShopOrderId,
                SkinAndCharacterBundleId = d.SkinAndCharacterBundleId,
                ItemId = d.ItemId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            });
            return dtos.ApplyQuery(query, (d, search) => d.ItemId.ToString().Contains(search));
        }

        public async Task<IEnumerable<TopUpHistoryDto>> GetAllTopUpHistoriesAsync(QueryParameters query)
        {
            var histories = await _topUpHistoryRepository.GetAllAsync();
            var dtos = histories.Select(h => new TopUpHistoryDto
            {
                TopUpId = h.TopUpId,
                UserId = h.UserId,
                GemBundleId = h.GemBundleId,
                GemsAmount = h.GemsAmount,
                RealMoneyAmount = h.RealMoneyAmount,
                CurrencyCode = h.CurrencyCode,
                PaymentGateway = h.PaymentGateway,
                TransactionId = h.TransactionId,
                Status = h.Status,
                Date = h.Date
            });
            return dtos.ApplyQuery(query, (h, search) => h.TransactionId.Contains(search, StringComparison.OrdinalIgnoreCase) || h.Status.Contains(search, StringComparison.OrdinalIgnoreCase));
        }
    }
}
