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

        public async Task<User?> GetByResetTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            return await _dbSet.FirstOrDefaultAsync(u => u.PasswordResetToken == token);
        }

        public async Task<int> CountOnlineUsersAsync(DateTime threshold)
        {
            return await _dbSet.CountAsync(u => u.IsOnline == 1);
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

        public async Task SetInactiveUsersOfflineAsync(DateTime threshold)
        {
            var inactiveUsers = await _dbSet
            .Where(u => u.IsOnline == 1 && (!u.LastOnline.HasValue || u.LastOnline < threshold))
                .ToListAsync();

            if (inactiveUsers.Any())
            {
                foreach (var user in inactiveUsers)
                {
                    user.IsOnline = 0;
                }
                await _context.SaveChangesAsync();
            }
        }
    }

    public class UserItemRepository : Repository<UserItem>, IUserItemRepository
    {
        public UserItemRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<UserItem>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(ui => ui.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserItem>> GetByUserIdWithNamesAsync(int userId)
        {
            return await _dbSet.Where(ui => ui.UserId == userId)
                               .Include(ui => ui.User)
                               .Include(ui => ui.Item)
                               .ToListAsync();
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

        public async Task<Staff?> GetByResetTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            return await _dbSet.FirstOrDefaultAsync(s => s.PasswordResetToken == token);
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

        public async Task<ShopOrder?> GetByOrderCodeAsync(long orderCode)
        {
            return await _dbSet
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
        }

        public async Task<IEnumerable<ShopOrder>> GetCompletedOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet.Where(o => o.OrderDate >= start && o.OrderDate <= end)
                               .ToListAsync();
        }

        public async Task<IEnumerable<ShopOrder>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(o => o.UserId == userId)
                               .OrderByDescending(o => o.OrderDate)
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

    public class OrderTransactionRepository : Repository<OrderTransaction>, IOrderTransactionRepository
    {
        public OrderTransactionRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<OrderTransaction>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet.Where(t => t.TransactionDateTime >= start && t.TransactionDateTime <= end)
                               .ToListAsync();
        }

        public async Task<IEnumerable<OrderTransaction>> GetByOrderIdsAsync(IEnumerable<int> orderIds)
        {
            var idList = orderIds.ToList();
            return await _dbSet.Where(t => idList.Contains(t.OrderId))
                               .OrderByDescending(t => t.TransactionDateTime)
                               .ToListAsync();
        }
    }

    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(UnaliveDbContext context) : base(context) { }

        public async Task<IEnumerable<PurchaseOrder>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Bundle)
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Bundle)
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PurchaseOrderId == id);
        }
    }
}
