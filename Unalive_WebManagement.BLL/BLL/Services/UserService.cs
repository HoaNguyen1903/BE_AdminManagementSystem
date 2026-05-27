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
        private readonly IShopOrderRepository _shopOrderRepository;
        private readonly IOrderTransactionRepository _orderTransactionRepository;

        public UserService(
            IUserRepository userRepository,
            IUserItemRepository userItemRepository,
            IUserBundleRepository userBundleRepository,
            IUserBanLogRepository userBanLogRepository,
            IShopOrderRepository shopOrderRepository,
            IOrderTransactionRepository orderTransactionRepository)
        {
            _userRepository = userRepository;
            _userItemRepository = userItemRepository;
            _userBundleRepository = userBundleRepository;
            _userBanLogRepository = userBanLogRepository;
            _shopOrderRepository = shopOrderRepository;
            _orderTransactionRepository = orderTransactionRepository;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(UserFilterParameters filter)
        {
            var users = await _userRepository.GetAllAsync();
            var now = DateTimeOffset.UtcNow;

            var query = users.AsQueryable();

            // Apply specific filters (AND logic)
            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                query = query.Where(u => u.UserName.Contains(filter.UserName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(u => u.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.IsOnline.HasValue)
            {
                query = query.Where(u => u.IsOnline == filter.IsOnline.Value);
            }

            if (filter.IsEmailVerified.HasValue)
            {
                query = query.Where(u => u.IsEmailVerified == filter.IsEmailVerified.Value);
            }

            var dtos = query.Select(u => MapToUserDto(u, now)).ToList();

            // Apply common query parameters (Search, SortBy, IsDescending)
            return dtos.ApplyQuery(filter, (u, search) =>
                u.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.UserName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return null;

            var now = DateTimeOffset.UtcNow;
            if (u.BannedUntil.HasValue && u.BannedUntil <= now)
            {
                u.BannedUntil = null;
                await _userRepository.UpdateAsync(u);
            }

            return MapToUserDto(u, now);
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
                LastOnline = DateTimeOffset.UtcNow,
                IsOnline = 0
            };
            var created = await _userRepository.AddAsync(user);
            return MapToUserDto(created, DateTimeOffset.UtcNow);
        }

        private static UserDto MapToUserDto(User u, DateTimeOffset now)
        {
            var isBanned = u.BannedUntil.HasValue && u.BannedUntil > now;
            return new UserDto
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Banned = isBanned ? (short)1 : (short)0,
                BannedUntil = isBanned ? u.BannedUntil : null,
                LastOnline = u.LastOnline,
                IsOnline = u.IsOnline,
                AvatarUrl = u.AvatarUrl,
                RankPoint = u.RankPoint
            };
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException();

            // Check if email is being changed and if new email already exists
            if (!string.IsNullOrEmpty(dto.Email) && !user.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email is already in use.");
                }
            }
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName))
                user.LastName = dto.LastName;

            if (!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;

            if (dto.LastOnline.HasValue)
                user.LastOnline = dto.LastOnline;

            if (!string.IsNullOrEmpty(dto.AvatarUrl))
                user.AvatarUrl = dto.AvatarUrl;

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password;

            if (dto.IsOnline.HasValue && dto.IsOnline != -1)
                user.IsOnline = dto.IsOnline.Value;

            if (dto.RankPoint.HasValue && dto.RankPoint != -1)
                user.RankPoint = dto.RankPoint.Value;

            // Set to null if past date or null
            user.BannedUntil = (dto.BannedUntil.HasValue && dto.BannedUntil.Value > DateTimeOffset.UtcNow)
                            ? dto.BannedUntil
                : null;

            user.Banned = user.BannedUntil.HasValue ? (short)1 : (short)0;

            await _userRepository.UpdateAsync(user);
        }

        public async Task BanUserAsync(int id, BanUserRequest dto, int staffId)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException();

            // If BannedUntil is null or in the past, lift the ban
            if (!dto.BannedUntil.HasValue || dto.BannedUntil.Value <= DateTimeOffset.UtcNow)
            {
                user.BannedUntil = null;
                user.Banned = 0;
            }
            else
            {
                user.BannedUntil = dto.BannedUntil;
                user.Banned = 1;
            }

            await _userRepository.UpdateAsync(user);

            var log = new UserBanLog
            {
                UserId = id,
                BanReason = dto.BanReason,
                BannedDate = DateTimeOffset.UtcNow,
                BannedUntil = user.BannedUntil,
                BannedBy = staffId
            };
            await _userBanLogRepository.AddAsync(log);
        }

        public async Task PermanentBanUserAsync(int id, string reason, int staffId)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException();

            user.BannedUntil = DateTimeOffset.UtcNow.AddYears(1000);
            user.Banned = 1;

            await _userRepository.UpdateAsync(user);

            var log = new UserBanLog
            {
                UserId = id,
                BanReason = reason,
                BannedDate = DateTimeOffset.UtcNow,
                BannedUntil = user.BannedUntil,
                BannedBy = staffId
            };
            await _userBanLogRepository.AddAsync(log);
        }

        public async Task UpdateUserLastOnlineAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException();
            user.LastOnline = DateTimeOffset.UtcNow;
            user.IsOnline = 1;
            await _userRepository.UpdateAsync(user);
        }

        public async Task SetUserOfflineAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException();
            user.IsOnline = 0;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserStatusDto?> GetUserStatusAsync(int userId, int onlineThresholdSeconds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            // Use the new IsOnline field directly
            return new UserStatusDto
            {
                UserId = user.UserId,
                IsOnline = user.IsOnline,
                LastOnline = user.LastOnline
            };
        }

        // UserBanLog CRUD
        public async Task<IEnumerable<UserBanLogDto>> GetAllUserBanLogsAsync(QueryParameters query)
        {
            var logs = await _userBanLogRepository.GetAllAsync();
            var dtos = logs.Select(MapToUserBanLogDto);
            return dtos.ApplyQuery(query, (l, search) => l.BanReason.Contains(search, StringComparison.OrdinalIgnoreCase) || l.UserId.ToString().Contains(search));
        }

        public async Task<IEnumerable<UserBanLogDto>> GetUserBanLogsByUserIdAsync(int userId, QueryParameters query)
        {
            var logs = await _userBanLogRepository.GetAllAsync();
            var dtos = logs.Where(l => l.UserId == userId).Select(MapToUserBanLogDto);
            return dtos.ApplyQuery(query, (l, search) => l.BanReason.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<UserBanLogDto> CreateUserBanLogAsync(CreateUserBanLogDto dto, int staffId)
        {
            var log = new UserBanLog
            {
                UserId = dto.UserId,
                BanReason = dto.BanReason,
                BannedDate = DateTimeOffset.UtcNow,
                BannedUntil = dto.BannedUntil,
                BannedBy = staffId
            };
            var created = await _userBanLogRepository.AddAsync(log);
            return MapToUserBanLogDto(created);
        }

        private static UserBanLogDto MapToUserBanLogDto(UserBanLog l)
        {
            return new UserBanLogDto
            {
                UserBanLogId = l.UserBanLogId,
                UserId = l.UserId,
                BanReason = l.BanReason,
                BannedDate = l.BannedDate,
                BannedUntil = l.BannedUntil,
                BannedBy = l.BannedBy
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
            var dtos = items.Select(MapToUserItemDto);
            return dtos.ApplyQuery(query, (i, search) => i.UserId.ToString().Contains(search) || i.ItemId.ToString().Contains(search));
        }

        public async Task<IEnumerable<UserItemDto>> GetUserItemsByUserIdAsync(int userId, QueryParameters query)
        {
            var items = await _userItemRepository.GetByUserIdAsync(userId);
            var dtos = items.Select(MapToUserItemDto);
            return dtos.ApplyQuery(query, (i, search) => i.ItemId.ToString().Contains(search));
        }

        private static UserItemDto MapToUserItemDto(UserItem i)
        {
            return new UserItemDto
            {
                UserId = i.UserId,
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                ShopOrderId = i.ShopOrderId ?? 0,
                PurchaseOrderId = i.PurchaseOrderId ?? 0
            };
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
            var item = new UserItem { UserId = dto.UserId, ItemId = dto.ItemId, Quantity = dto.Quantity, ShopOrderId = dto.ShopOrderId, PurchaseOrderId = dto.PurchaseOrderId };
            var created = await _userItemRepository.AddAsync(item);
            return MapToUserItemDto(created);
        }

        public async Task<UserItemDto> CreateUserItemNoOrderAsync(CreateUserItemNoOrderDto dto)
        {
            var item = new UserItem { UserId = dto.UserId, ItemId = dto.ItemId, Quantity = dto.Quantity };
            var created = await _userItemRepository.AddAsync(item);
            return MapToUserItemDto(created);
        }

        public async Task UpdateUserItemAsync(int userId, int itemId, UpdateUserItemDto dto)
        {
            var item = await _userItemRepository.GetByIdAsync(userId, itemId);
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
            var dtos = bundles.Select(MapToUserBundleDto);
            return dtos.ApplyQuery(query, (b, search) => b.UserId.ToString().Contains(search));
        }

        public async Task<IEnumerable<UserBundleDto>> GetUserBundlesByUserIdAsync(int userId, QueryParameters query)
        {
            var bundles = await _userBundleRepository.GetAllAsync();
            var dtos = bundles.Where(b => b.UserId == userId).Select(MapToUserBundleDto);
            return dtos.ApplyQuery(query, (b, search) => b.SkinAndCharacterBundleId.ToString().Contains(search) || b.GemBundleId.ToString().Contains(search));
        }

        private static UserBundleDto MapToUserBundleDto(UserBundle b)
        {
            return new UserBundleDto
            {
                UserId = b.UserId,
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                GemBundleId = b.GemBundleId,
                Remaining = b.Remaining
            };
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
            return MapToUserBundleDto(created);
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

        public async Task<PlayerProfileDto?> GetPlayerProfileAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return null;

            var inventory = await GetUserItemsWithNamesByUserIdAsync(userId, new QueryParameters());
            var orders = await _shopOrderRepository.GetByUserIdAsync(userId);

            var orderDtos = orders.Select(o => new ShopOrderDto
            {
                ShopOrderId = o.ShopOrderId,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                PlayerEmail = o.PlayerEmail,
                PlayerUserName = o.PlayerUserName,
                OrderCode = o.OrderCode,
                Currency = o.Currency ?? "VND"
            }).ToList();

            var orderIds = orders.Select(o => o.ShopOrderId).ToList();
            var transactions = await _orderTransactionRepository.GetByOrderIdsAsync(orderIds);
            var banLogs = await GetUserBanLogsByUserIdAsync(userId, new QueryParameters());

            return new PlayerProfileDto
            {
                User = user,
                Inventory = inventory,
                Orders = orderDtos,
                Transactions = transactions,
                BanLogs = banLogs
            };
        }

        public async Task<long> GetRankPointAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user.RankPoint;
        }

        public async Task<long> UpdateRankPointAsync(int userId, long points, bool isAddition)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (isAddition)
            {
                user.RankPoint += points;
            }
            else
            {
                user.RankPoint -= points;
                if (user.RankPoint < 0) user.RankPoint = 0; // Prevent negative rank points
            }

            await _userRepository.UpdateAsync(user);
            return user.RankPoint;
        }
    }
}
