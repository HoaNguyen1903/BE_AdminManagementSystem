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
        private readonly IUserBanLogRepository _userBanLogRepository;

        public UserService(
            IUserRepository userRepository, 
            IUserItemRepository userItemRepository, 
            IUserBundleRepository userBundleRepository,
            IUserBanLogRepository userBanLogRepository)
        {
            _userRepository = userRepository;
            _userItemRepository = userItemRepository;
            _userBundleRepository = userBundleRepository;
            _userBanLogRepository = userBanLogRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(QueryParameters query)
        {
            var users = await _userRepository.GetAllAsync();
            var now = DateTime.UtcNow;

            var dtos = users.Select(u => {
                var isBanned = u.BannedUntil.HasValue && u.BannedUntil > now;
                return new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Banned = isBanned,
                    BannedUntil = isBanned ? u.BannedUntil : null,
                    LastOnline = u.LastOnline,
                    AvatarUrl = u.AvatarUrl
                };
            });

            return dtos.ApplyQuery(query, (u, search) =>
                u.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.UserName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return null;
            
            var now = DateTime.UtcNow;
            var isBanned = u.BannedUntil.HasValue && u.BannedUntil > now;
            
            // If ban has expired, update database to null
            if (u.BannedUntil.HasValue && !isBanned)
            {
                u.BannedUntil = null;
                await _userRepository.UpdateAsync(u);
            }

            return new UserDto
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Banned = isBanned,
                BannedUntil = isBanned ? u.BannedUntil : null,
                LastOnline = u.LastOnline,
                AvatarUrl = u.AvatarUrl
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                BannedUntil = null,
                LastOnline = DateTime.UtcNow
            };
            var created = await _userRepository.AddAsync(user);
            return new UserDto
            {
                UserId = created.UserId,
                Email = created.Email,
                FirstName = created.FirstName,
                LastName = created.LastName,
                UserName = created.UserName,
                Banned = false,
                BannedUntil = null,
                LastOnline = created.LastOnline,
                AvatarUrl = created.AvatarUrl
            };
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException();

            // Check if email is being changed and if new email already exists
            if (!user.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email is already in use.");
                }
            }

            user.Email = dto.Email;
            user.Password = dto.Password;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.LastOnline = dto.LastOnline;
            user.AvatarUrl = dto.AvatarUrl;
            
            // Set to null if past date or null
            user.BannedUntil = (dto.BannedUntil.HasValue && dto.BannedUntil.Value > DateTime.UtcNow) 
                ? dto.BannedUntil 
                : null;
                
            await _userRepository.UpdateAsync(user);
        }

        public async Task BanUserAsync(int id, BanUserRequest dto, int staffId)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException();

            // Set to null if past date or null
            user.BannedUntil = (dto.BannedUntil.HasValue && dto.BannedUntil.Value > DateTime.UtcNow) 
                ? dto.BannedUntil 
                : null;

            await _userRepository.UpdateAsync(user);

            var log = new UserBanLog
            {
                UserId = id,
                BanReason = dto.BanReason,
                BannedDate = DateTime.UtcNow,
                BannedUntil = user.BannedUntil, // Log what was actually set
                BannedBy = staffId
            };
            await _userBanLogRepository.AddAsync(log);
        }

        public async Task UpdateUserLastOnlineAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException();
            user.LastOnline = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserStatusDto?> GetUserStatusAsync(int userId, int onlineThresholdSeconds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var now = DateTime.UtcNow;
            var isOnline = user.LastOnline.HasValue &&
                           user.LastOnline.Value >= now.AddSeconds(-onlineThresholdSeconds);

            return new UserStatusDto
            {
                UserId = user.UserId,
                IsOnline = isOnline,
                LastOnline = user.LastOnline
            };
        }

        // UserBanLog CRUD
        public async Task<IEnumerable<UserBanLogDto>> GetAllUserBanLogsAsync(QueryParameters query)
        {
            var logs = await _userBanLogRepository.GetAllAsync();
            var dtos = logs.Select(l => new UserBanLogDto
            {
                UserBanLogId = l.UserBanLogId,
                UserId = l.UserId,
                BanReason = l.BanReason,
                BannedDate = l.BannedDate,
                BannedUntil = l.BannedUntil,
                BannedBy = l.BannedBy
            });
            return dtos.ApplyQuery(query, (l, search) => l.BanReason.Contains(search, StringComparison.OrdinalIgnoreCase) || l.UserId.ToString().Contains(search));
        }

        public async Task<IEnumerable<UserBanLogDto>> GetUserBanLogsByUserIdAsync(int userId, QueryParameters query)
        {
            var logs = await _userBanLogRepository.GetAllAsync();
            var dtos = logs.Where(l => l.UserId == userId).Select(l => new UserBanLogDto
            {
                UserBanLogId = l.UserBanLogId,
                UserId = l.UserId,
                BanReason = l.BanReason,
                BannedDate = l.BannedDate,
                BannedUntil = l.BannedUntil,
                BannedBy = l.BannedBy
            });
            return dtos.ApplyQuery(query, (l, search) => l.BanReason.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<UserBanLogDto> CreateUserBanLogAsync(CreateUserBanLogDto dto, int staffId)
        {
            var log = new UserBanLog
            {
                UserId = dto.UserId,
                BanReason = dto.BanReason,
                BannedDate = DateTime.UtcNow,
                BannedUntil = dto.BannedUntil,
                BannedBy = staffId
            };
            var created = await _userBanLogRepository.AddAsync(log);
            return new UserBanLogDto
            {
                UserBanLogId = created.UserBanLogId,
                UserId = created.UserId,
                BanReason = created.BanReason,
                BannedDate = created.BannedDate,
                BannedUntil = created.BannedUntil,
                BannedBy = created.BannedBy
            };
        }

        public async Task UpdateUserBanLogAsync(int id, UpdateUserBanLogDto dto)
        {
            var log = await _userBanLogRepository.GetByIdAsync(id);
            if (log == null) throw new KeyNotFoundException();
            log.BanReason = dto.BanReason;
            log.BannedUntil = dto.BannedUntil;
            await _userBanLogRepository.UpdateAsync(log);
        }

        public async Task DeleteUserBanLogAsync(int id)
        {
            await _userBanLogRepository.DeleteAsync(id);
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

        public async Task<IEnumerable<UserItemWithNameDto>> GetUserItemsWithNamesByUserIdAsync(int userId, QueryParameters query)
        {
            var items = await _userItemRepository.GetByUserIdWithNamesAsync(userId);
            var dtos = items.Select(i => new UserItemWithNameDto
            {
                UserId = i.UserId,
                UserName = i.User?.UserName ?? "Unknown",
                ItemId = i.ItemId,
                ItemName = i.Item?.ItemName ?? "Unknown",
                Quantity = i.Quantity
            });
            return dtos.ApplyQuery(query, (i, search) => 
                i.ItemName.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                i.ItemId.ToString().Contains(search));
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
            var bundle = new UserBundle
            {
                UserId = dto.UserId,
                SkinAndCharacterBundleId = dto.SkinAndCharacterBundleId > 0 ? dto.SkinAndCharacterBundleId : null,
                GemBundleId = dto.GemBundleId > 0 ? dto.GemBundleId : null,
                Remaining = dto.Remaining
            };

            var created = await _userBundleRepository.AddAsync(bundle);

            return new UserBundleDto
            {
                UserId = created.UserId,
                SkinAndCharacterBundleId = created.SkinAndCharacterBundleId,
                GemBundleId = created.GemBundleId,
                Remaining = created.Remaining
            };
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

        public async Task UpdateAvatarAsync(int userId, string avatarUrl)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return;
            user.AvatarUrl = avatarUrl;
            await _userRepository.UpdateAsync(user);
        }
    }
}
