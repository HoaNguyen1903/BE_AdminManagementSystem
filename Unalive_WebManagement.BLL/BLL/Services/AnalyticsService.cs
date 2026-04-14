using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IShopOrderRepository _shopOrderRepository;
        private readonly IShopOrderDetailRepository _shopOrderDetailRepository;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;
        private readonly IGemBundleRepository _gemBundleRepository;
        private readonly ISkinAndCharacterBundleRepository _skinBundleRepository;
        private readonly IUserRepository _userRepository;

        public AnalyticsService(
            IShopOrderRepository shopOrderRepository,
            IShopOrderDetailRepository shopOrderDetailRepository,
            ITopUpHistoryRepository topUpHistoryRepository,
            IGemBundleRepository gemBundleRepository,
            ISkinAndCharacterBundleRepository skinBundleRepository,
            IUserRepository userRepository)
        {
            _shopOrderRepository = shopOrderRepository;
            _shopOrderDetailRepository = shopOrderDetailRepository;
            _topUpHistoryRepository = topUpHistoryRepository;
            _gemBundleRepository = gemBundleRepository;
            _skinBundleRepository = skinBundleRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<RevenueAnalyticsDto>> GetRevenueAnalyticsAsync(DateTime start, DateTime end, string groupBy)
        {
            var orders = await _shopOrderRepository.GetCompletedOrdersByDateRangeAsync(start, end);
            var topUps = await _topUpHistoryRepository.GetCompletedTopUpsByDateRangeAsync(start, end);

            var dailyRevenue = new Dictionary<string, decimal>();

            foreach (var order in orders)
            {
                var key = groupBy == "month"
                    ? order.OrderDate.ToString("yyyy-MM")
                    : order.OrderDate.ToString("yyyy-MM-dd");

                if (!dailyRevenue.ContainsKey(key))
                    dailyRevenue[key] = 0;

                dailyRevenue[key] += (decimal)order.TotalAmount;
            }

            foreach (var topUp in topUps)
            {
                var key = groupBy == "month"
                    ? topUp.Date.ToString("yyyy-MM")
                    : topUp.Date.ToString("yyyy-MM-dd");

                if (!dailyRevenue.ContainsKey(key))
                    dailyRevenue[key] = 0;

                dailyRevenue[key] += (decimal)topUp.RealMoneyAmount;
            }

            return dailyRevenue
                .OrderBy(x => x.Key)
                .Select(x => new RevenueAnalyticsDto { Label = x.Key, Revenue = x.Value })
                .ToList();
        }

        public async Task<IEnumerable<BundleRankingDto>> GetBundleRankingAsync(DateTime start, DateTime end, int top)
        {
            var orders = await _shopOrderRepository.GetCompletedOrdersByDateRangeAsync(start, end);
            var topUps = await _topUpHistoryRepository.GetCompletedTopUpsByDateRangeAsync(start, end);

            var gemBundleIds = topUps.Select(t => t.GemBundleId).Distinct().ToList();
            var gemBundles = (await _gemBundleRepository.GetAllWithIdsAsync(gemBundleIds))
                .ToDictionary(g => g.GemBundleId, g => g.BundleName);

            var orderIds = orders.Select(o => o.ShopOrderId).ToList();
            var details = (await _shopOrderDetailRepository.GetDetailsByOrderIdsAsync(orderIds)).ToList();
            var skinBundleIds = details.Where(d => d.SkinAndCharacterBundleId.HasValue)
                .Select(d => d.SkinAndCharacterBundleId!.Value).Distinct().ToList();
            var skinBundles = (await _skinBundleRepository.GetAllWithIdsAsync(skinBundleIds))
                .ToDictionary(s => s.SkinAndCharacterBundleId, s => s.BundleName);

            var gemRevenue = new Dictionary<int, (string Name, decimal Revenue, int Count)>();
            var skinRevenue = new Dictionary<int, (string Name, decimal Revenue, int Count)>();

            foreach (var topUp in topUps)
            {
                var bundleName = gemBundles.ContainsKey(topUp.GemBundleId)
                    ? gemBundles[topUp.GemBundleId]
                    : topUp.GemBundleId.ToString();

                if (!gemRevenue.ContainsKey(topUp.GemBundleId))
                {
                    gemRevenue[topUp.GemBundleId] = (bundleName, 0, 0);
                }
                var current = gemRevenue[topUp.GemBundleId];
                gemRevenue[topUp.GemBundleId] = (current.Name, current.Revenue + (decimal)topUp.RealMoneyAmount, current.Count + 1);
            }

            foreach (var detail in details)
            {
                if (detail.SkinAndCharacterBundleId.HasValue)
                {
                    var bundleId = detail.SkinAndCharacterBundleId.Value;
                    var bundleName = skinBundles.ContainsKey(bundleId)
                        ? skinBundles[bundleId]
                        : bundleId.ToString();

                    if (!skinRevenue.ContainsKey(bundleId))
                    {
                        skinRevenue[bundleId] = (bundleName, 0, 0);
                    }
                    var current = skinRevenue[bundleId];
                    skinRevenue[bundleId] = (current.Name, current.Revenue + (decimal)(detail.UnitPrice * detail.Quantity), current.Count + detail.Quantity);
                }
            }

            var result = new List<BundleRankingDto>();

            foreach (var kvp in gemRevenue)
            {
                result.Add(new BundleRankingDto
                {
                    BundleId = kvp.Key,
                    BundleName = kvp.Value.Name,
                    Type = "Gem",
                    Count = kvp.Value.Count,
                    Revenue = kvp.Value.Revenue
                });
            }

            foreach (var kvp in skinRevenue)
            {
                result.Add(new BundleRankingDto
                {
                    BundleId = kvp.Key,
                    BundleName = kvp.Value.Name,
                    Type = "Skin",
                    Count = kvp.Value.Count,
                    Revenue = kvp.Value.Revenue
                });
            }

            return result.OrderByDescending(x => x.Revenue).Take(top).ToList();
        }

        public async Task<PlayerStatsDto> GetPlayerStatsAsync(DateTime start, DateTime end)
        {
            var now = DateTime.UtcNow;
            var onlineThreshold = now.AddMinutes(-5);
            var today = now.Date;

            var currentOnline = await _userRepository.CountOnlineUsersAsync(onlineThreshold);
            var dau = await _userRepository.CountDailyActiveUsersAsync(today);
            var bannedCount = await _userRepository.CountBannedUsersAsync();

            return new PlayerStatsDto
            {
                CurrentOnline = currentOnline,
                DailyActiveUsers = dau,
                BannedAccountsCount = bannedCount
            };
        }
    }
}
