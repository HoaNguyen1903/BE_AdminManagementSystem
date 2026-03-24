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
                Name = i.Name,
                Price = i.Price,
                Description = i.Description,
                Type = i.Type
            });
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) return null;

            return new ItemDto
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                Type = item.Type
            };
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemDto dto)
        {
            var item = new Item
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Type = dto.Type
            };

            var created = await _itemRepository.AddAsync(item);

            return new ItemDto
            {
                ItemId = created.ItemId,
                Name = created.Name,
                Price = created.Price,
                Description = created.Description,
                Type = created.Type
            };
        }

        public async Task UpdateItemAsync(int id, UpdateItemDto dto)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException($"Item with ID {id} not found");

            item.Name = dto.Name;
            item.Price = dto.Price;
            item.Description = dto.Description;
            item.Type = dto.Type;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _itemRepository.DeleteAsync(id);
        }
    }
}
