using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;

namespace Unalive_WebManagement.BLL.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ISkinAndCharacterBundleRepository _skinBundleRepository;
        private readonly IGemBundleRepository _gemBundleRepository;

        public ItemService(
            IItemRepository itemRepository,
            IAnnouncementRepository announcementRepository,
            INotificationRepository notificationRepository,
            ISkinAndCharacterBundleRepository skinBundleRepository,
            IGemBundleRepository gemBundleRepository)
        {
            _itemRepository = itemRepository;
            _announcementRepository = announcementRepository;
            _notificationRepository = notificationRepository;
            _skinBundleRepository = skinBundleRepository;
            _gemBundleRepository = gemBundleRepository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync(ItemFilterParameters query)
        {
            var items = await _itemRepository.GetAllAsync();
            var q = items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                q = q.Where(i => i.Status.ToString().Equals(query.Status, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.ItemType))
            {
                q = q.Where(i => i.ItemType.Contains(query.ItemType, StringComparison.OrdinalIgnoreCase));
            }

            var dtos = q.Select(i => new ItemDto
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                ItemDescription = i.ItemDescription,
                ItemType = i.ItemType,
                ItemImageUrl = i.ItemImageUrl,
                Status = i.Status.ToString()
            });

            return dtos.ApplyQuery(query, (i, s) =>
                i.ItemName.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                i.ItemDescription.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                i.ItemType.Contains(s, StringComparison.OrdinalIgnoreCase));
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
                ItemType = item.ItemType,
                ItemImageUrl = item.ItemImageUrl,
                Status = item.Status.ToString()
            };
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemDto dto, int staffId)
        {
            // Backend Guard: Prevent duplicate items by name
            var items = await _itemRepository.GetAllAsync();
            if (items.Any(i => i.ItemName.Equals(dto.ItemName, StringComparison.OrdinalIgnoreCase) && i.Status == Item.StatusEnum.Available))
            {
                throw new InvalidOperationException($"An active item with the name '{dto.ItemName}' already exists.");
            }

            var item = new Item
            {
                ItemName = dto.ItemName,
                ItemDescription = dto.ItemDescription,
                ItemType = dto.ItemType,
                ItemImageUrl = dto.ItemImageUrl,
                Status = Enum.TryParse<Item.StatusEnum>(dto.Status, true, out var s) ? s : Item.StatusEnum.Available
            };

            var created = await _itemRepository.AddAsync(item);

            // Create automatic announcement
            var announcement = new Announcement
            {
                Title = $"[{created.ItemName}] added to the game",
                Content = "",
                Type = "General",
                Status = "Drafted",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                CreatedBy = staffId,
                CreatedAt = DateTime.UtcNow
            };
            await _announcementRepository.AddAsync(announcement);

            // Create notification to staff
            var notification = new Notification
            {
                NotificationMessage = "There's an announcement waiting to be edited or published.",
                ReceiverId = staffId,
                Read = 0
            };
            await _notificationRepository.AddAsync(notification);

            return new ItemDto
            {
                ItemId = created.ItemId,
                ItemName = created.ItemName,
                ItemDescription = created.ItemDescription,
                ItemType = created.ItemType,
                ItemImageUrl = created.ItemImageUrl,
                Status = created.Status.ToString()
            };
        }

        public async Task UpdateItemAsync(int id, UpdateItemDto dto)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException($"Item with ID {id} not found");

            item.ItemName = dto.ItemName;
            item.ItemDescription = dto.ItemDescription;
            item.ItemType = dto.ItemType;
            item.ItemImageUrl = dto.ItemImageUrl;
            if (dto.Status != null && Enum.TryParse<Item.StatusEnum>(dto.Status, true, out var status))
                item.Status = status;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task UpdateItemStatusAsync(int id, string status)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException();

            if (Enum.TryParse<Item.StatusEnum>(status, true, out var s))
            {
                item.Status = s;
                await _itemRepository.UpdateAsync(item);

                // Cascade disable bundles if item becomes unavailable
                if (s == Item.StatusEnum.Unavailable)
                {
                    await DisableAssociatedBundlesAsync(id);
                }
            }
            else
            {
                throw new ArgumentException("Invalid status");
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException();

            item.Status = Item.StatusEnum.Unavailable;
            await _itemRepository.UpdateAsync(item);
            
            // Cascade disable bundles
            await DisableAssociatedBundlesAsync(id);
        }

        public async Task<AssociatedBundlesDto> GetAssociatedBundlesAsync(int itemId)
        {
            var skinBundles = await _skinBundleRepository.GetAllAsync();
            var gemBundles = await _gemBundleRepository.GetAllAsync();

            var associatedSkinBundles = skinBundles.Where(b => b.ItemId == itemId).Select(b => new SkinAndCharacterBundleDto
            {
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice,
                ItemId = b.ItemId,
                Quantity = b.Quantity,
                imageUrl = b.imageUrl,
                Status = b.Status.ToString()
            });

            var associatedGemBundles = gemBundles.Where(b => b.ItemId == itemId).Select(b => new GemBundleDto
            {
                GemBundleId = b.GemBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice,
                ItemId = b.ItemId,
                Quantity = b.Quantity,
                imageUrl = b.imageUrl,
                Status = b.Status.ToString()
            });

            return new AssociatedBundlesDto
            {
                SkinAndCharacterBundles = associatedSkinBundles,
                GemBundles = associatedGemBundles
            };
        }

        private async Task DisableAssociatedBundlesAsync(int itemId)
        {
            var skinBundles = await _skinBundleRepository.GetAllAsync();
            var gemBundles = await _gemBundleRepository.GetAllAsync();

            var associatedSkinBundles = skinBundles.Where(b => b.ItemId == itemId && b.Status == SkinAndCharacterBundle.StatusEnum.Available);
            foreach (var bundle in associatedSkinBundles)
            {
                bundle.Status = SkinAndCharacterBundle.StatusEnum.Unavailable;
                await _skinBundleRepository.UpdateAsync(bundle);
            }

            var associatedGemBundles = gemBundles.Where(b => b.ItemId == itemId && b.Status == GemBundle.StatusEnum.Available);
            foreach (var bundle in associatedGemBundles)
            {
                bundle.Status = GemBundle.StatusEnum.Unavailable;
                await _gemBundleRepository.UpdateAsync(bundle);
            }
        }
    }
}