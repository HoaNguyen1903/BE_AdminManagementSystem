using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Interfaces
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerDto>> GetAllBannersAsync();
        Task<BannerDto?> GetBannerByIdAsync(int id);
        Task<BannerDto> CreateBannerAsync(CreateBannerDto dto);
        Task UpdateBannerAsync(int id, UpdateBannerDto dto);
        Task DeleteBannerAsync(int id);
    }

    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto);
        Task UpdateItemAsync(int id, UpdateItemDto dto);
        Task DeleteItemAsync(int id);
    }

    public interface IBannerItemService
    {
        Task<IEnumerable<BannerItemDto>> GetAllBannerItemsAsync();
        Task<BannerItemDto?> GetBannerItemByIdAsync(int bannerId, int itemId);
        Task<BannerItemDto> CreateBannerItemAsync(CreateBannerItemDto dto);
        Task DeleteBannerItemAsync(int bannerId, int itemId);
    }

    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task UpdateUserAsync(int id, UpdateUserDto dto);
        Task DeleteUserAsync(int id);
    }

    public interface IUserItemService
    {
        Task<IEnumerable<UserItemDto>> GetAllUserItemsAsync();
        Task<IEnumerable<UserItemDto>> GetUserItemsByUserIdAsync(int userId);
        Task<UserItemDto?> GetUserItemByIdAsync(int id);
        Task<UserItemDto> CreateUserItemAsync(CreateUserItemDto dto);
        Task UpdateUserItemAsync(int id, UpdateUserItemDto dto);
        Task DeleteUserItemAsync(int id);
    }

    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
        Task<NotificationDto?> GetNotificationByIdAsync(int id);
    }

    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
    }

    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync();
        Task<OrderDetailDto?> GetOrderDetailByIdAsync(int orderId, int productId);
    }

    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        Task<ReportDto?> GetReportByIdAsync(int id);
        Task<bool> ApproveReportAsync(int reportId, int staffId);
    }

    public interface IStaffService
    {
        Task<IEnumerable<StaffDto>> GetAllStaffAsync();
        Task<StaffDto?> GetStaffByIdAsync(int id);
        Task<StaffDto?> GetStaffByEmailAsync(string email);
    }

    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
