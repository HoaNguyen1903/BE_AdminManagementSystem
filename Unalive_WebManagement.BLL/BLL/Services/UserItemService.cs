using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class UserItemService : IUserItemService
    {
        private readonly IUserItemRepository _userItemRepository;

        public UserItemService(IUserItemRepository userItemRepository)
        {
            _userItemRepository = userItemRepository;
        }

        public async Task<IEnumerable<UserItemDto>> GetAllUserItemsAsync()
        {
            var userItems = await _userItemRepository.GetAllAsync();
            return userItems.Select(ui => new UserItemDto
            {
                UserItemId = ui.UserItemId,
                UserId = ui.UserId,
                ItemId = ui.ItemId,
                Quantity = ui.Quantity,
                ObtainedDate = ui.ObtainedDate
            });
        }

        public async Task<IEnumerable<UserItemDto>> GetUserItemsByUserIdAsync(int userId)
        {
            var userItems = await _userItemRepository.GetByUserIdAsync(userId);
            return userItems.Select(ui => new UserItemDto
            {
                UserItemId = ui.UserItemId,
                UserId = ui.UserId,
                ItemId = ui.ItemId,
                Quantity = ui.Quantity,
                ObtainedDate = ui.ObtainedDate
            });
        }

        public async Task<UserItemDto?> GetUserItemByIdAsync(int id)
        {
            var userItem = await _userItemRepository.GetByIdAsync(id);
            if (userItem == null) return null;

            return new UserItemDto
            {
                UserItemId = userItem.UserItemId,
                UserId = userItem.UserId,
                ItemId = userItem.ItemId,
                Quantity = userItem.Quantity,
                ObtainedDate = userItem.ObtainedDate
            };
        }

        public async Task<UserItemDto> CreateUserItemAsync(CreateUserItemDto dto)
        {
            var userItem = new UserItem
            {
                UserId = dto.UserId,
                ItemId = dto.ItemId,
                Quantity = dto.Quantity,
                ObtainedDate = DateTime.Now // Or from DTO if needed, usually set on creation
            };

            var created = await _userItemRepository.AddAsync(userItem);

            return new UserItemDto
            {
                UserItemId = created.UserItemId,
                UserId = created.UserId,
                ItemId = created.ItemId,
                Quantity = created.Quantity,
                ObtainedDate = created.ObtainedDate
            };
        }

        public async Task UpdateUserItemAsync(int id, UpdateUserItemDto dto)
        {
            var userItem = await _userItemRepository.GetByIdAsync(id);
            if (userItem == null) throw new KeyNotFoundException($"UserItem with ID {id} not found");

            userItem.UserId = dto.UserId;
            userItem.ItemId = dto.ItemId;
            userItem.Quantity = dto.Quantity;
            // ObtainedDate usually doesn't change on update, but depends on requirements

            await _userItemRepository.UpdateAsync(userItem);
        }

        public async Task DeleteUserItemAsync(int id)
        {
            await _userItemRepository.DeleteAsync(id);
        }
    }
}
