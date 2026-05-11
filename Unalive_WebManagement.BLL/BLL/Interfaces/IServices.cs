using Microsoft.AspNetCore.Http;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse?> LoginUserAsync(LoginRequest request);
        Task<LoginResponse?> RegisterUserAsync(RegisterRequest request);
        Task<bool> VerifyEmailAsync(int userId, string token);
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendVerificationEmailAsync(string to, string userName, string verificationLink);
    }

    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync(ItemFilterParameters query);
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto, int staffId);
        Task UpdateItemAsync(int id, UpdateItemDto dto);
        Task UpdateItemStatusAsync(int id, string status);
        Task DeleteItemAsync(int id);
    }

    public interface ICharacterService
    {
        Task<IEnumerable<CharacterStatDto>> GetAllCharacterStatsAsync(QueryParameters query);
        Task<CharacterStatDto?> GetCharacterStatByIdAsync(int id);
        Task<CharacterStatDto> CreateCharacterStatAsync(CreateCharacterStatDto dto);
        Task UpdateCharacterStatAsync(int id, UpdateCharacterStatDto dto);

        Task<IEnumerable<CharacterPvPDto>> GetAllCharacterPvPsAsync(QueryParameters query);
        Task<CharacterPvPDto?> GetCharacterPvPByIdAsync(int id);
        Task<CharacterPvPDto> CreateCharacterPvPAsync(CreateCharacterPvPDto dto);
        Task UpdateCharacterPvPAsync(int id, UpdateCharacterPvPDto dto);

        Task<IEnumerable<CharacterPassiveDto>> GetAllCharacterPassivesAsync(CharacterFilterParameters query);
        Task<CharacterPassiveDto?> GetCharacterPassiveByIdAsync(int id);
        Task<CharacterPassiveDto> CreateCharacterPassiveAsync(CreateCharacterPassiveDto dto);
        Task UpdateCharacterPassiveAsync(int id, UpdateCharacterPassiveDto dto);

        Task<IEnumerable<CharacterSkillDto>> GetAllCharacterSkillsAsync(CharacterFilterParameters query);
        Task<CharacterSkillDto?> GetCharacterSkillByIdAsync(int id);
        Task<CharacterSkillDto> CreateCharacterSkillAsync(CreateCharacterSkillDto dto);
        Task UpdateCharacterSkillAsync(int id, UpdateCharacterSkillDto dto);

        Task<IEnumerable<CharacterAttackDto>> GetAllCharacterAttacksAsync(CharacterFilterParameters query);
        Task<CharacterAttackDto?> GetCharacterAttackByIdAsync(int id);
        Task<CharacterAttackDto> CreateCharacterAttackAsync(CreateCharacterAttackDto dto);
        Task UpdateCharacterAttackAsync(int id, UpdateCharacterAttackDto dto);

        Task<AbilitiesSetDto?> GetAbilitiesSetByTacticIdAsync(int id);
    }

    public interface IShopService
    {
        Task<IEnumerable<GemBundleDto>> GetAllGemBundlesAsync(BundleFilterParameters query);
        Task<GemBundleDto?> GetGemBundleByIdAsync(int id);
        Task<GemBundleDto> CreateGemBundleAsync(CreateGemBundleDto dto, int staffId);
        Task UpdateGemBundleAsync(int id, UpdateGemBundleDto dto);
        Task UpdateGemBundleStatusAsync(int id, string status);
        Task DeleteGemBundleAsync(int id);


        Task<IEnumerable<SkinAndCharacterBundleDto>> GetAllSkinAndCharacterBundlesAsync(BundleFilterParameters query);
        Task<SkinAndCharacterBundleDto?> GetSkinAndCharacterBundleByIdAsync(int id);
        Task<SkinAndCharacterBundleDto> CreateSkinAndCharacterBundleAsync(CreateSkinAndCharacterBundleDto dto, int staffId);
        Task UpdateSkinAndCharacterBundleAsync(int id, UpdateSkinAndCharacterBundleDto dto);
        Task UpdateSkinAndCharacterBundleStatusAsync(int id, string status);
        Task DeleteSkinAndCharacterBundleAsync(int id);

        Task<IEnumerable<ShopOrderDto>> GetAllShopOrdersAsync(QueryParameters query);
        
        Task<ShopOrder?> GetShopOrderByIdAsync(int id);
        Task<ShopOrder?> GetShopOrderByPaymentLinkIdAsync(string paymentLinkId);
        Task UpdateShopOrderAsync(int id, ShopOrder updatedOrder);
        Task<ShopOrder> CreateShopOrderAsync(ShopOrder order);
        Task<ShopOrder?> GetShopOrderByOrderCodeAsync(long orderCode);

        Task<IEnumerable<ShopOrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId, QueryParameters query);
        Task<IEnumerable<ShopOrderDetailDto>> GetDetailedOrderDetailsByOrderIdAsync(int orderId);
        Task<IEnumerable<TopUpHistoryDto>> GetAllTopUpHistoriesAsync(QueryParameters query);

        Task PurchaseGemBundleAsync(int userId, int bundleId);
        Task PurchaseSkinBundleAsync(int userId, int bundleId);
        Task ProcessSuccessfulOrderAsync(ShopOrder order);
    }

    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync(AnnouncementFilterParameters query);
        Task<AnnouncementDto?> GetAnnouncementByIdAsync(int id);
        Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto dto, int staffId);
        Task<AnnouncementDto> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto dto, int staffId);
        Task DeleteAnnouncementAsync(int id);
    }

    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(UserFilterParameters filter);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task UpdateUserAsync(int id, UpdateUserDto dto);
        Task BanUserAsync(int id, BanUserRequest dto, int staffId);
        Task UpdateUserLastOnlineAsync(int userId);
        Task SetUserOfflineAsync(int userId);
        Task<UserStatusDto?> GetUserStatusAsync(int userId, int onlineThresholdSeconds);
        Task UpdateAvatarAsync(int userId, string avatarUrl);
        Task<PlayerProfileDto?> GetPlayerProfileAsync(int userId);


        Task<IEnumerable<UserBanLogDto>> GetAllUserBanLogsAsync(QueryParameters query);
        Task<IEnumerable<UserBanLogDto>> GetUserBanLogsByUserIdAsync(int userId, QueryParameters query);
        Task<UserBanLogDto> CreateUserBanLogAsync(CreateUserBanLogDto dto, int staffId);
        Task UpdateUserBanLogAsync(int id, UpdateUserBanLogDto dto);
        Task DeleteUserBanLogAsync(int id);

        Task<IEnumerable<UserItemDto>> GetAllUserItemsAsync(QueryParameters query);
        Task<IEnumerable<UserItemDto>> GetUserItemsByUserIdAsync(int userId, QueryParameters query);
        Task<IEnumerable<UserItemWithNameDto>> GetUserItemsWithNamesByUserIdAsync(int userId, QueryParameters query);
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
        Task<IEnumerable<RevenueAnalyticsDto>> GetRevenueAnalyticsAsync(DateTime? start, DateTime? end, string groupBy);
        Task<IEnumerable<BundleRankingDto>> GetBundleRankingAsync(DateTime? start, DateTime? end, int top);
        Task<PlayerStatsDto> GetPlayerStatsAsync(DateTime? start, DateTime? end);
        Task<IEnumerable<TopSpenderDto>> GetTopSpendersAsync(DateTime? start, DateTime? end, int top);
    }

    public interface IOrderTransactionService
    {
        Task<IEnumerable<OrderTransaction>> GetAllTransactionsAsync();
        Task<OrderTransaction?> GetTransactionByIdAsync(int id);
        Task<IEnumerable<OrderTransaction>> GetTransactionsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderTransaction>> GetTransactionsByOrderCodeAsync(long orderCode);
        Task<IEnumerable<OrderTransaction>> GetTransactionsByPaymentLinkIdAsync(string paymentLinkId);
        Task<OrderTransaction> CreateTransactionAsync(OrderTransaction transaction);
        Task CreateTransactionsAsync(IEnumerable<OrderTransaction> transactions);
        Task UpdateTransactionAsync(int id, OrderTransaction updatedTransaction);
        Task DeleteTransactionAsync(int id);
        Task DeleteTransactionsByOrderIdAsync(int orderId);
    }

    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id);

        Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder order);
        Task UpdatePurchaseOrderAsync(int id, PurchaseOrder updatedOrder);
        Task DeletePurchaseOrderAsync(int id);
        Task<PurchaseOrder> ProcessBundlePurchaseAsync(int userId, int bundleId);
    }
}
