using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.DAL.Interfaces
{
    public interface IBannerRepository : IRepository<Banner> { }
    public interface IItemRepository : IRepository<Item> { }
    public interface IBannerItemRepository : IRepository<BannerItem> { }
    public interface IUserRepository : IRepository<User> { }
    public interface IUserItemRepository : IRepository<UserItem>
    {
        Task<IEnumerable<UserItem>> GetByUserIdAsync(int userId);
    }
    public interface INotificationRepository : IRepository<Notification> { }
    public interface IOrderRepository : IRepository<Order> { }
    public interface IOrderDetailRepository : IRepository<OrderDetail> { }
    public interface IReportRepository : IRepository<Report> { }
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetByEmailAsync(string email);
    }
}

