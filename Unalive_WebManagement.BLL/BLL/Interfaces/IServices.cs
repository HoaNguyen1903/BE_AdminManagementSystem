using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
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
        Task<IEnumerable<ReportDto>> GetAllReportsAsync(QueryParameters query);
        Task<ReportDto?> GetReportByIdAsync(int id);
    }
}
