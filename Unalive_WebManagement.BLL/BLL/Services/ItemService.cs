using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(i => new ItemDto
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                ItemDescription = i.ItemDescription,
                ItemType = i.ItemType
            });
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) return null;

            return new ItemDto
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                ItemType = item.ItemType
            };
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemDto dto)
        {
            var item = new Item
            {
                ItemName = dto.ItemName,
                ItemDescription = dto.ItemDescription,
                ItemType = dto.ItemType
            };

            var created = await _itemRepository.AddAsync(item);

            return new ItemDto
            {
                ItemId = created.ItemId,
                ItemName = created.ItemName,
                ItemDescription = created.ItemDescription,
                ItemType = created.ItemType
            };
        }

        public async Task UpdateItemAsync(int id, UpdateItemDto dto)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException($"Item with ID {id} not found");

            item.ItemName = dto.ItemName;
            item.ItemDescription = dto.ItemDescription;
            item.ItemType = dto.ItemType;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _itemRepository.DeleteAsync(id);
        }
    }
}
