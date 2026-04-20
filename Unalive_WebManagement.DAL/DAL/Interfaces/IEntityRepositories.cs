using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item> { }
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<int> CountOnlineUsersAsync(DateTime threshold);
        Task<int> CountDailyActiveUsersAsync(DateTime date);
        Task<int> CountBannedUsersAsync(DateTime? bannedUntilAfter = null);
    }
    public interface IUserItemRepository : IRepository<UserItem>
    {
        Task<IEnumerable<UserItem>> GetByUserIdAsync(int userId);
    }
    public interface INotificationRepository : IRepository<Notification> { }
    public interface IReportRepository : IRepository<Report> { }
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetByEmailAsync(string email);
    }
    public interface IAbilitiesSetRepository : IRepository<AbilitiesSet> { }
    public interface IAnnouncementRepository : IRepository<Announcement> { }
    public interface IBundleItemRepository : IRepository<BundleItem> { }
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
    public interface IOrderTransactionRepository : IRepository<OrderTransaction> { }

}
