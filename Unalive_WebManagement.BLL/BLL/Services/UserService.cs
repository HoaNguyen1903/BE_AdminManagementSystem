using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;

namespace Unalive_WebManagement.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserItemRepository _userItemRepository;
        private readonly IUserBundleRepository _userBundleRepository;

        public UserService(IUserRepository userRepository, IUserItemRepository userItemRepository, IUserBundleRepository userBundleRepository)
        {
            _userRepository = userRepository;
            _userItemRepository = userItemRepository;
            _userBundleRepository = userBundleRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(QueryParameters query)
        {
            var users = await _userRepository.GetAllAsync();
            var dtos = users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Banned = u.Banned
            });
            return dtos.ApplyQuery(query, (u, search) => 
                u.Email.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                u.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                u.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return null;
            return new UserDto { UserId = u.UserId, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName, Banned = u.Banned };
        }

        // UserItem CRUD
        public async Task<IEnumerable<UserItemDto>> GetAllUserItemsAsync(QueryParameters query)
        {
            var items = await _userItemRepository.GetAllAsync();
            var dtos = items.Select(i => new UserItemDto
            {
                UserId = i.UserId,
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                ShopOrderId = i.ShopOrderId
            });
            return dtos.ApplyQuery(query, (i, search) => i.UserId.ToString().Contains(search) || i.ItemId.ToString().Contains(search));
        }

        public async Task<IEnumerable<UserItemDto>> GetUserItemsByUserIdAsync(int userId, QueryParameters query)
        {
            var items = await _userItemRepository.GetByUserIdAsync(userId);
            var dtos = items.Select(i => new UserItemDto
            {
                UserId = i.UserId,
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                ShopOrderId = i.ShopOrderId
            });
            return dtos.ApplyQuery(query, (i, search) => i.ItemId.ToString().Contains(search));
        }

        public async Task<UserItemDto> CreateUserItemAsync(CreateUserItemDto dto)
        {
            var item = new UserItem { UserId = dto.UserId, ItemId = dto.ItemId, Quantity = dto.Quantity, ShopOrderId = dto.ShopOrderId };
            var created = await _userItemRepository.AddAsync(item);
            return new UserItemDto { UserId = created.UserId, ItemId = created.ItemId, Quantity = created.Quantity, ShopOrderId = created.ShopOrderId };
        }

        public async Task UpdateUserItemAsync(int userId, int itemId, UpdateUserItemDto dto)
        {
            var items = await _userItemRepository.GetAllAsync();
            var item = items.FirstOrDefault(i => i.UserId == userId && i.ItemId == itemId);
            if (item == null) throw new KeyNotFoundException();
            item.Quantity = dto.Quantity;
            item.ShopOrderId = dto.ShopOrderId;
            await _userItemRepository.UpdateAsync(item);
        }

        public async Task DeleteUserItemAsync(int userId, int itemId)
        {
            await _userItemRepository.DeleteAsync(userId, itemId);
        }

        // UserBundle CRUD
        public async Task<IEnumerable<UserBundleDto>> GetAllUserBundlesAsync(QueryParameters query)
        {
            var bundles = await _userBundleRepository.GetAllAsync();
            var dtos = bundles.Select(b => new UserBundleDto
            {
                UserId = b.UserId,
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                GemBundleId = b.GemBundleId,
                Remaining = b.Remaining
            });
            return dtos.ApplyQuery(query, (b, search) => b.UserId.ToString().Contains(search));
        }

        public async Task<IEnumerable<UserBundleDto>> GetUserBundlesByUserIdAsync(int userId, QueryParameters query)
        {
            var bundles = await _userBundleRepository.GetAllAsync();
            var dtos = bundles.Where(b => b.UserId == userId).Select(b => new UserBundleDto
            {
                UserId = b.UserId,
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                GemBundleId = b.GemBundleId,
                Remaining = b.Remaining
            });
            return dtos.ApplyQuery(query, (b, search) => b.SkinAndCharacterBundleId.ToString().Contains(search) || b.GemBundleId.ToString().Contains(search));
        }

        public async Task<UserBundleDto> CreateUserBundleAsync(CreateUserBundleDto dto)
        {
            var bundle = new UserBundle { UserId = dto.UserId, SkinAndCharacterBundleId = dto.SkinAndCharacterBundleId, GemBundleId = dto.GemBundleId, Remaining = dto.Remaining };
            var created = await _userBundleRepository.AddAsync(bundle);
            return new UserBundleDto { UserId = created.UserId, SkinAndCharacterBundleId = created.SkinAndCharacterBundleId, GemBundleId = created.GemBundleId, Remaining = created.Remaining };
        }

        public async Task UpdateUserBundleAsync(int userId, int skinBundleId, int gemBundleId, UpdateUserBundleDto dto)
        {
            var skinBundleIdNullable = skinBundleId == 0 ? null : (int?)skinBundleId;
            var gemBundleIdNullable = gemBundleId == 0 ? null : (int?)gemBundleId;
            
            var bundle = await _userBundleRepository.GetByIdAsync(userId, skinBundleIdNullable, gemBundleIdNullable);
            if (bundle == null) throw new KeyNotFoundException();
            bundle.Remaining = dto.Remaining;
            await _userBundleRepository.UpdateAsync(bundle);
        }

        public async Task DeleteUserBundleAsync(int userId, int skinBundleId, int gemBundleId)
        {
            var skinBundleIdNullable = skinBundleId == 0 ? null : (int?)skinBundleId;
            var gemBundleIdNullable = gemBundleId == 0 ? null : (int?)gemBundleId;
            
            await _userBundleRepository.DeleteAsync(userId, skinBundleIdNullable, gemBundleIdNullable);
        }
    }
}
