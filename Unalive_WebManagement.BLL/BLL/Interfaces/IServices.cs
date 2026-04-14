using Microsoft.AspNetCore.Http;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse?> LoginUserAsync(LoginRequest request);
        Task<LoginResponse?> RegisterUserAsync(RegisterRequest request);
    }

    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync(QueryParameters query);
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto, int staffId);
        Task UpdateItemAsync(int id, UpdateItemDto dto);
        Task DeleteItemAsync(int id);
    }

    public interface ICharacterService
    {
        Task<IEnumerable<CharacterStatDto>> GetAllCharacterStatsAsync(QueryParameters query);
        Task<CharacterStatDto?> GetCharacterStatByIdAsync(int id);
        Task<IEnumerable<CharacterPvPDto>> GetAllCharacterPvPsAsync(QueryParameters query);
        Task<CharacterPvPDto?> GetCharacterPvPByIdAsync(int id);
        Task<IEnumerable<CharacterPassiveDto>> GetAllCharacterPassivesAsync(QueryParameters query);
        Task<CharacterPassiveDto?> GetCharacterPassiveByIdAsync(int id);
        Task<IEnumerable<CharacterSkillDto>> GetAllCharacterSkillsAsync(QueryParameters query);
        Task<CharacterSkillDto?> GetCharacterSkillByIdAsync(int id);
        Task<IEnumerable<CharacterAttackDto>> GetAllCharacterAttacksAsync(QueryParameters query);
        Task<CharacterAttackDto?> GetCharacterAttackByIdAsync(int id);
        Task<AbilitiesSetDto?> GetAbilitiesSetByTacticIdAsync(int id);
    }

    public interface IShopService
    {
        Task<IEnumerable<GemBundleDto>> GetAllGemBundlesAsync(QueryParameters query);
        Task<GemBundleDto?> GetGemBundleByIdAsync(int id);
        Task<GemBundleDto> CreateGemBundleAsync(CreateGemBundleDto dto, int staffId);
        Task UpdateGemBundleAsync(int id, UpdateGemBundleDto dto);
        Task DeleteGemBundleAsync(int id);

        Task<IEnumerable<SkinAndCharacterBundleDto>> GetAllSkinAndCharacterBundlesAsync(QueryParameters query);
        Task<SkinAndCharacterBundleDto?> GetSkinAndCharacterBundleByIdAsync(int id);
        Task<SkinAndCharacterBundleDto> CreateSkinAndCharacterBundleAsync(CreateSkinAndCharacterBundleDto dto, int staffId);
        Task UpdateSkinAndCharacterBundleAsync(int id, UpdateSkinAndCharacterBundleDto dto);
        Task DeleteSkinAndCharacterBundleAsync(int id);

        Task<IEnumerable<BundleItemDto>> GetAllBundleItemsAsync(QueryParameters query);
        Task<IEnumerable<BundleItemDto>> GetItemsByBundleIdAsync(int bundleId);
        Task<BundleItemDto> CreateBundleItemAsync(CreateBundleItemDto dto);
        Task UpdateBundleItemAsync(int bundleId, int itemId, UpdateBundleItemDto dto);
        Task DeleteBundleItemAsync(int bundleId, int itemId);

        Task<IEnumerable<ShopOrderDto>> GetAllShopOrdersAsync(QueryParameters query);
        Task<IEnumerable<ShopOrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId, QueryParameters query);
        Task<IEnumerable<TopUpHistoryDto>> GetAllTopUpHistoriesAsync(QueryParameters query);

        Task PurchaseGemBundleAsync(int userId, int bundleId);
        Task PurchaseSkinBundleAsync(int userId, int bundleId);
    }

    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync(QueryParameters query);
        Task<AnnouncementDto?> GetAnnouncementByIdAsync(int id);
        Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto dto, int staffId);
        Task UpdateAnnouncementAsync(int id, UpdateAnnouncementDto dto, int staffId);
        Task DeleteAnnouncementAsync(int id);
    }

    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync(QueryParameters query);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task UpdateUserAsync(int id, UpdateUserDto dto);
        Task BanUserAsync(int id, BanUserRequest dto, int staffId);
        Task UpdateUserLastOnlineAsync(int userId);
        Task<UserStatusDto?> GetUserStatusAsync(int userId, int onlineThresholdSeconds);
        Task UpdateAvatarAsync(int userId, string avatarUrl);


        Task<IEnumerable<UserBanLogDto>> GetAllUserBanLogsAsync(QueryParameters query);
        Task<IEnumerable<UserBanLogDto>> GetUserBanLogsByUserIdAsync(int userId, QueryParameters query);
        Task<UserBanLogDto> CreateUserBanLogAsync(CreateUserBanLogDto dto, int staffId);
        Task UpdateUserBanLogAsync(int id, UpdateUserBanLogDto dto);
        Task DeleteUserBanLogAsync(int id);

        Task<IEnumerable<UserItemDto>> GetAllUserItemsAsync(QueryParameters query);
        Task<IEnumerable<UserItemDto>> GetUserItemsByUserIdAsync(int userId, QueryParameters query);
        Task<UserItemDto> CreateUserItemAsync(CreateUserItemDto dto);
        Task UpdateUserItemAsync(int userId, int itemId, UpdateUserItemDto dto);
        Task DeleteUserItemAsync(int userId, int itemId);

        Task<IEnumerable<UserBundleDto>> GetAllUserBundlesAsync(QueryParameters query);
        Task<IEnumerable<UserBundleDto>> GetUserBundlesByUserIdAsync(int userId, QueryParameters query);
        Task<UserBundleDto> CreateUserBundleAsync(CreateUserBundleDto dto);
        Task UpdateUserBundleAsync(int userId, int skinBundleId, int gemBundleId, UpdateUserBundleDto dto);
        Task DeleteUserBundleAsync(int userId, int skinBundleId, int gemBundleId);
    }

    public interface IFeedbackService
    {
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync(QueryParameters query);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId, QueryParameters query);
        Task MarkNotificationReadAsync(int id);
        Task MarkNotificationUnreadAsync(int id);
        Task<IEnumerable<ReportDto>> GetAllReportsAsync(QueryParameters query);
        Task<ReportDto?> GetReportByIdAsync(int id);
        Task<ReportDto> CreateReportAsync(CreateReportDto dto, int senderId);
        Task ApproveReportAsync(int id, int staffId);
    }

    public interface IBlobService
    {
        Task<string> UploadImageAsync(IFormFile file, string fileName);
        Task DeleteImageAsync(string fileName);
    }

    public interface IAnalyticsService
    {
        Task<IEnumerable<RevenueAnalyticsDto>> GetRevenueAnalyticsAsync(DateTime start, DateTime end, string groupBy);
        Task<IEnumerable<BundleRankingDto>> GetBundleRankingAsync(DateTime start, DateTime end, int top);
        Task<PlayerStatsDto> GetPlayerStatsAsync(DateTime start, DateTime end);
    }
}
