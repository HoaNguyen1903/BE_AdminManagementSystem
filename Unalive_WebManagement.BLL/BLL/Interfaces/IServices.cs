using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }

    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto);
        Task UpdateItemAsync(int id, UpdateItemDto dto);
        Task DeleteItemAsync(int id);
    }

    public interface ICharacterService
    {
        Task<IEnumerable<CharacterStatDto>> GetAllCharacterStatsAsync();
        Task<CharacterStatDto?> GetCharacterStatByIdAsync(int id);
        Task<IEnumerable<CharacterPvPDto>> GetAllCharacterPvPsAsync();
        Task<CharacterPvPDto?> GetCharacterPvPByIdAsync(int id);
        Task<IEnumerable<CharacterPassiveDto>> GetAllCharacterPassivesAsync();
        Task<CharacterPassiveDto?> GetCharacterPassiveByIdAsync(int id);
        Task<IEnumerable<CharacterSkillDto>> GetAllCharacterSkillsAsync();
        Task<CharacterSkillDto?> GetCharacterSkillByIdAsync(int id);
        Task<IEnumerable<CharacterAttackDto>> GetAllCharacterAttacksAsync();
        Task<CharacterAttackDto?> GetCharacterAttackByIdAsync(int id);
        Task<AbilitiesSetDto?> GetAbilitiesSetByTacticIdAsync(int id);
    }

    public interface IShopService
    {
        Task<IEnumerable<GemBundleDto>> GetAllGemBundlesAsync();
        Task<IEnumerable<SkinAndCharacterBundleDto>> GetAllSkinAndCharacterBundlesAsync();
        Task<IEnumerable<ShopOrderDto>> GetAllShopOrdersAsync();
        Task<IEnumerable<ShopOrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<IEnumerable<TopUpHistoryDto>> GetAllTopUpHistoriesAsync();
    }
}
