using Microsoft.EntityFrameworkCore;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.Data;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.DAL.Repositories
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        public BannerRepository(UnaliveDbContext context) : base(context) { }
    }

    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(UnaliveDbContext context) : base(context) { }
    }

    public class BannerItemRepository : Repository<BannerItem>, IBannerItemRepository
    {
        public BannerItemRepository(UnaliveDbContext context) : base(context) { }
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UnaliveDbContext context) : base(context) { }
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

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(UnaliveDbContext context) : base(context) { }
    }

    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(UnaliveDbContext context) : base(context) { }
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
}

