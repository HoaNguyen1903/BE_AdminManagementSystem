using Microsoft.EntityFrameworkCore;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.Data;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.DAL.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(UnaliveDbContext context) : base(context) { }
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UnaliveDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var trimmedEmail = email.Trim();
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.Trim() == trimmedEmail);
        }

        public async Task<int> CountOnlineUsersAsync(DateTime threshold)
        {
            return await _dbSet.CountAsync(u => u.LastOnline >= threshold);
        }

        public async Task<int> CountDailyActiveUsersAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _dbSet.CountAsync(u => u.LastOnline >= dateOnly);
        }

        public async Task<int> CountBannedUsersAsync(DateTime? bannedUntilAfter = null)
        {
            if (bannedUntilAfter.HasValue)
            {
                return await _dbSet.CountAsync(u => u.BannedUntil > bannedUntilAfter.Value);
            }
            return await _dbSet.CountAsync(u => u.BannedUntil == null || u.BannedUntil > DateTime.UtcNow);
        }
    }

    public class UserItemRepository : Repository<UserItem>, IUserItemRepository
    {
        public UserItemRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<UserItem>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(ui => ui.UserId == userId).ToListAsync();
        }
    }

    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(UnaliveDbContext context) : base(context) { }
    }

    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(UnaliveDbContext context) : base(context) { }
    }

    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(UnaliveDbContext context) : base(context) { }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var trimmedEmail = email.Trim();
            return await _dbSet.FirstOrDefaultAsync(s => s.Email.Trim() == trimmedEmail);
        }
    }

    public class AbilitiesSetRepository : Repository<AbilitiesSet>, IAbilitiesSetRepository
    {
        public AbilitiesSetRepository(UnaliveDbContext context) : base(context) { }
    }

    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(UnaliveDbContext context) : base(context) { }
    }

    public class BundleItemRepository : Repository<BundleItem>, IBundleItemRepository
    {
        public BundleItemRepository(UnaliveDbContext context) : base(context) { }
    }

    public class CharacterAttackRepository : Repository<CharacterAttack>, ICharacterAttackRepository
    {
        public CharacterAttackRepository(UnaliveDbContext context) : base(context) { }
    }

    public class CharacterPassiveRepository : Repository<CharacterPassive>, ICharacterPassiveRepository
    {
        public CharacterPassiveRepository(UnaliveDbContext context) : base(context) { }
    }

    public class CharacterPvPRepository : Repository<CharacterPvP>, ICharacterPvPRepository
    {
        public CharacterPvPRepository(UnaliveDbContext context) : base(context) { }
    }

    public class CharacterSkillRepository : Repository<CharacterSkill>, ICharacterSkillRepository
    {
        public CharacterSkillRepository(UnaliveDbContext context) : base(context) { }
    }

    public class CharacterStatRepository : Repository<CharacterStat>, ICharacterStatRepository
    {
        public CharacterStatRepository(UnaliveDbContext context) : base(context) { }
    }

    public class GemBundleRepository : Repository<GemBundle>, IGemBundleRepository
    {
        public GemBundleRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<GemBundle>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.ToList();
            return await _dbSet.Where(g => idList.Contains(g.GemBundleId)).ToListAsync();
        }
    }

    public class SkinAndCharacterBundleRepository : Repository<SkinAndCharacterBundle>, ISkinAndCharacterBundleRepository
    {
        public SkinAndCharacterBundleRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<SkinAndCharacterBundle>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.ToList();
            return await _dbSet.Where(s => idList.Contains(s.SkinAndCharacterBundleId)).ToListAsync();
        }
    }

    public class ShopOrderRepository : Repository<ShopOrder>, IShopOrderRepository
    {
        public ShopOrderRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<ShopOrder>> GetCompletedOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet.Where(o => o.OrderDate >= start && o.OrderDate <= end)
                               .ToListAsync();
        }
    }

    public class ShopOrderDetailRepository : Repository<ShopOrderDetail>, IShopOrderDetailRepository
    {
        public ShopOrderDetailRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<ShopOrderDetail>> GetDetailsByOrderIdsAsync(IEnumerable<int> orderIds)
        {
            var idList = orderIds.ToList();
            return await _dbSet.Where(d => idList.Contains(d.ShopOrderId))
                               .ToListAsync();
        }
    }

    public class TopUpHistoryRepository : Repository<TopUpHistory>, ITopUpHistoryRepository
    {
        public TopUpHistoryRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<TopUpHistory>> GetCompletedTopUpsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet.Where(t => t.Status == "Completed" && t.Date >= start && t.Date <= end)
                               .ToListAsync();
        }
    }

    public class UserBundleRepository : Repository<UserBundle>, IUserBundleRepository
    {
        public UserBundleRepository(UnaliveDbContext context) : base(context) { }
    }

    public class UserBanLogRepository : Repository<UserBanLog>, IUserBanLogRepository
    {
        public UserBanLogRepository(UnaliveDbContext context) : base(context) { }
    }

    public class PlayerOnlineHistoryRepository : Repository<PlayerOnlineHistory>, IPlayerOnlineHistoryRepository
    {
        public PlayerOnlineHistoryRepository(UnaliveDbContext context) : base(context) { }

        public async Task<PlayerOnlineHistory?> GetByDateAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _dbSet.FirstOrDefaultAsync(h => h.Date.Date == dateOnly);
        }

        public async Task<IEnumerable<PlayerOnlineHistory>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet.Where(h => h.Date >= start && h.Date <= end)
                               .OrderBy(h => h.Date)
                               .ToListAsync();
        }
    }
}
