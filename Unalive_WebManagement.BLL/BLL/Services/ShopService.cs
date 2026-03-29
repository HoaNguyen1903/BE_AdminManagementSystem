using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly IGemBundleRepository _gemBundleRepository;
        private readonly ISkinAndCharacterBundleRepository _skinAndCharacterBundleRepository;
        private readonly IShopOrderRepository _shopOrderRepository;
        private readonly IShopOrderDetailRepository _shopOrderDetailRepository;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;

        public ShopService(
            IGemBundleRepository gemBundleRepository,
            ISkinAndCharacterBundleRepository skinAndCharacterBundleRepository,
            IShopOrderRepository shopOrderRepository,
            IShopOrderDetailRepository shopOrderDetailRepository,
            ITopUpHistoryRepository topUpHistoryRepository)
        {
            _gemBundleRepository = gemBundleRepository;
            _skinAndCharacterBundleRepository = skinAndCharacterBundleRepository;
            _shopOrderRepository = shopOrderRepository;
            _shopOrderDetailRepository = shopOrderDetailRepository;
            _topUpHistoryRepository = topUpHistoryRepository;
        }

        public async Task<IEnumerable<GemBundleDto>> GetAllGemBundlesAsync()
        {
            var bundles = await _gemBundleRepository.GetAllAsync();
            return bundles.Select(b => new GemBundleDto
            {
                GemBundleId = b.GemBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice
            });
        }

        public async Task<IEnumerable<SkinAndCharacterBundleDto>> GetAllSkinAndCharacterBundlesAsync()
        {
            var bundles = await _skinAndCharacterBundleRepository.GetAllAsync();
            return bundles.Select(b => new SkinAndCharacterBundleDto
            {
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice
            });
        }

        public async Task<IEnumerable<ShopOrderDto>> GetAllShopOrdersAsync()
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            return orders.Select(o => new ShopOrderDto
            {
                ShopOrderId = o.ShopOrderId,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate
            });
        }

        public async Task<IEnumerable<ShopOrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var details = await _shopOrderDetailRepository.GetAllAsync();
            return details.Where(d => d.ShopOrderId == orderId).Select(d => new ShopOrderDetailDto
            {
                ShopOrderDetailId = d.ShopOrderDetailId,
                ShopOrderId = d.ShopOrderId,
                SkinAndCharacterBundleId = d.SkinAndCharacterBundleId,
                ItemId = d.ItemId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            });
        }

        public async Task<IEnumerable<TopUpHistoryDto>> GetAllTopUpHistoriesAsync()
        {
            var histories = await _topUpHistoryRepository.GetAllAsync();
            return histories.Select(h => new TopUpHistoryDto
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
        }
    }
}
