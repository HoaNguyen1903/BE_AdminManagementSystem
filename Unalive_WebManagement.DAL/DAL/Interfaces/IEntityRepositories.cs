using Unalive_WebManagement.Models; // Make sure to include this for PurchaseOrder

namespace Unalive_WebManagement.DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item> { }
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByResetTokenAsync(string token);
        Task<int> CountOnlineUsersAsync(DateTime threshold);
        Task<int> CountDailyActiveUsersAsync(DateTime date);
        Task<int> CountBannedUsersAsync(DateTime? bannedUntilAfter = null);
        Task SetInactiveUsersOfflineAsync(DateTime threshold);
    }
    public interface IUserItemRepository : IRepository<UserItem>
    {
        Task<IEnumerable<UserItem>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserItem>> GetByUserIdWithNamesAsync(int userId);
    }
    public interface INotificationRepository : IRepository<Notification> { }
    public interface IReportRepository : IRepository<Report> { }
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetByEmailAsync(string email);
        Task<Staff?> GetByResetTokenAsync(string token);
    }
    public interface IAbilitiesSetRepository : IRepository<AbilitiesSet> { }
    public interface IAnnouncementRepository : IRepository<Announcement> { }
    public interface ICharacterAttackRepository : IRepository<CharacterAttack> { }
    public interface ICharacterPassiveRepository : IRepository<CharacterPassive> { }
    public interface ICharacterPvPRepository : IRepository<CharacterPvP> { }
    public interface ICharacterSkillRepository : IRepository<CharacterSkill> { }
    public interface ICharacterStatRepository : IRepository<CharacterStat> { }
    public interface IGemBundleRepository : IRepository<GemBundle>
    {
        Task<IEnumerable<GemBundle>> GetAllWithIdsAsync(IEnumerable<int> ids);
    }
    public interface ISkinAndCharacterBundleRepository : IRepository<SkinAndCharacterBundle>
    {
        Task<IEnumerable<SkinAndCharacterBundle>> GetAllWithIdsAsync(IEnumerable<int> ids);
    }
    public interface IShopOrderRepository : IRepository<ShopOrder>
    {
        Task<IEnumerable<ShopOrder>> GetCompletedOrdersByDateRangeAsync(DateTime start, DateTime end);
        Task<ShopOrder?> GetByOrderCodeAsync(long orderCode);
        Task<IEnumerable<ShopOrder>> GetByUserIdAsync(int userId);
    }
    public interface IShopOrderDetailRepository : IRepository<ShopOrderDetail>
    {
        Task<IEnumerable<ShopOrderDetail>> GetDetailsByOrderIdsAsync(IEnumerable<int> orderIds);
    }
    public interface ITopUpHistoryRepository : IRepository<TopUpHistory>
    {
        Task<IEnumerable<TopUpHistory>> GetCompletedTopUpsByDateRangeAsync(DateTime start, DateTime end);
    }
    public interface IUserBundleRepository : IRepository<UserBundle> { }
    public interface IUserBanLogRepository : IRepository<UserBanLog> { }
    public interface IPlayerOnlineHistoryRepository : IRepository<PlayerOnlineHistory>
    {
        Task<PlayerOnlineHistory?> GetByDateAsync(DateTime date);
        Task<IEnumerable<PlayerOnlineHistory>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
    public interface IOrderTransactionRepository : IRepository<OrderTransaction>
    {
        Task<IEnumerable<OrderTransaction>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<OrderTransaction>> GetByOrderIdsAsync(IEnumerable<int> orderIds);
    }

    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<IEnumerable<PurchaseOrder>> GetAllWithDetailsAsync();
        Task<PurchaseOrder?> GetByIdWithDetailsAsync(int id);
    }
}