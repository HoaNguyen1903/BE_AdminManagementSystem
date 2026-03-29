using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item> { }
    public interface IUserRepository : IRepository<User> { }
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
    public interface IGemBundleRepository : IRepository<GemBundle> { }
    public interface ISkinAndCharacterBundleRepository : IRepository<SkinAndCharacterBundle> { }
    public interface IShopOrderRepository : IRepository<ShopOrder> { }
    public interface IShopOrderDetailRepository : IRepository<ShopOrderDetail> { }
    public interface ITopUpHistoryRepository : IRepository<TopUpHistory> { }
    public interface IUserBundleRepository : IRepository<UserBundle> { }
}
