using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;
using PayOS.Models.V2.PaymentRequests;

namespace Unalive_WebManagement.BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly IGemBundleRepository _gemBundleRepository;
        private readonly ISkinAndCharacterBundleRepository _skinAndCharacterBundleRepository;
        private readonly IShopOrderRepository _shopOrderRepository;
        private readonly IShopOrderDetailRepository _shopOrderDetailRepository;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserBundleRepository _userBundleRepository;
        private readonly IUserItemRepository _userItemRepository;

        public ShopService(
            IGemBundleRepository gemBundleRepository,
            ISkinAndCharacterBundleRepository skinAndCharacterBundleRepository,
            IShopOrderRepository shopOrderRepository,
            IShopOrderDetailRepository shopOrderDetailRepository,
            ITopUpHistoryRepository topUpHistoryRepository,
            IAnnouncementRepository announcementRepository,
            INotificationRepository notificationRepository,
            IUserBundleRepository userBundleRepository,
            IUserItemRepository userItemRepository)
        {
            _gemBundleRepository = gemBundleRepository;
            _skinAndCharacterBundleRepository = skinAndCharacterBundleRepository;
            _shopOrderRepository = shopOrderRepository;
            _shopOrderDetailRepository = shopOrderDetailRepository;
            _topUpHistoryRepository = topUpHistoryRepository;
            _announcementRepository = announcementRepository;
            _notificationRepository = notificationRepository;
            _userBundleRepository = userBundleRepository;
            _userItemRepository = userItemRepository;
        }

        // GemBundle CRUD
        public async Task<IEnumerable<GemBundleDto>> GetAllGemBundlesAsync(QueryParameters query)
        {
            var bundles = await _gemBundleRepository.GetAllAsync();
            var dtos = bundles.Select(b => new GemBundleDto
            {
                GemBundleId = b.GemBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice,
                ItemId = b.ItemId,
                Quantity = b.Quantity
            });
            return dtos.ApplyQuery(query, (b, search) => b.BundleName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<GemBundleDto?> GetGemBundleByIdAsync(int id)
        {
            var b = await _gemBundleRepository.GetByIdAsync(id);
            if (b == null) return null;
            return new GemBundleDto { GemBundleId = b.GemBundleId, BundleName = b.BundleName, BundlePrice = b.BundlePrice };
        }

        public async Task<GemBundleDto> CreateGemBundleAsync(CreateGemBundleDto dto)
        {
            var bundle = new GemBundle
            {
                BundleName = dto.BundleName,
                BundlePrice = dto.BundlePrice,
                ItemId = dto.ItemId,
                Quantity = dto.Quantity
            };
            var created = await _gemBundleRepository.AddAsync(bundle);
            return new GemBundleDto
            {
                GemBundleId = created.GemBundleId,
                BundleName = created.BundleName,
                BundlePrice = created.BundlePrice,
                ItemId = created.ItemId,
                Quantity = created.Quantity
            };
        }

        public async Task<GemBundleDto> CreateGemBundleAsync(CreateGemBundleDto dto, int staffId)
        {
            var created = await CreateGemBundleAsync(dto);

            // Create automatic announcement
            var announcement = new Announcement
            {
                Title = $"[{created.BundleName}] added to the game",
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
                Read = false
            };
            await _notificationRepository.AddAsync(notification);

            return created;
        }

        public async Task UpdateGemBundleAsync(int id, UpdateGemBundleDto dto)
        {
            var bundle = await _gemBundleRepository.GetByIdAsync(id);
            if (bundle == null) throw new KeyNotFoundException();

            if (dto.BundleName != null) bundle.BundleName = dto.BundleName;
            if (dto.BundlePrice != null) bundle.BundlePrice = dto.BundlePrice.Value;
            if (dto.ItemId != null) bundle.ItemId = dto.ItemId.Value;
            if (dto.Quantity != null) bundle.Quantity = dto.Quantity.Value;

            await _gemBundleRepository.UpdateAsync(bundle);
        }

        public async Task DeleteGemBundleAsync(int id) => await _gemBundleRepository.DeleteAsync(id);

        // SkinAndCharacterBundle CRUD
        public async Task<IEnumerable<SkinAndCharacterBundleDto>> GetAllSkinAndCharacterBundlesAsync(QueryParameters query)
        {
            var bundles = await _skinAndCharacterBundleRepository.GetAllAsync();
            var dtos = bundles.Select(b => new SkinAndCharacterBundleDto
            {
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice,
                ItemId = b.ItemId,
                Quantity = b.Quantity
            });
            return dtos.ApplyQuery(query, (b, search) => b.BundleName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<SkinAndCharacterBundleDto?> GetSkinAndCharacterBundleByIdAsync(int id)
        {
            var b = await _skinAndCharacterBundleRepository.GetByIdAsync(id);
            if (b == null) return null;
            return new SkinAndCharacterBundleDto
            {
                SkinAndCharacterBundleId = b.SkinAndCharacterBundleId,
                BundleName = b.BundleName,
                BundlePrice = b.BundlePrice,
                ItemId = b.ItemId,
                Quantity = b.Quantity
            };
        }

        public async Task<SkinAndCharacterBundleDto> CreateSkinAndCharacterBundleAsync(CreateSkinAndCharacterBundleDto dto, int staffId)
        {
            var bundle = new SkinAndCharacterBundle
            {
                BundleName = dto.BundleName,
                BundlePrice = dto.BundlePrice,
                ItemId = dto.ItemId,
                Quantity = dto.Quantity
            };
            var created = await _skinAndCharacterBundleRepository.AddAsync(bundle);

            // Create automatic announcement
            var announcement = new Announcement
            {
                Title = $"[{created.BundleName}] added to the game",
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
                Read = false
            };
            await _notificationRepository.AddAsync(notification);

            return new SkinAndCharacterBundleDto
            {
                SkinAndCharacterBundleId = created.SkinAndCharacterBundleId,
                BundleName = created.BundleName,
                BundlePrice = created.BundlePrice,
                ItemId = created.ItemId,
                Quantity = created.Quantity
            };
        }

        public async Task UpdateSkinAndCharacterBundleAsync(int id, UpdateSkinAndCharacterBundleDto dto)
        {
            var bundle = await _skinAndCharacterBundleRepository.GetByIdAsync(id);
            if (bundle == null) throw new KeyNotFoundException();

            if (dto.BundleName != null) bundle.BundleName = dto.BundleName;
            if (dto.BundlePrice != null) bundle.BundlePrice = dto.BundlePrice.Value;
            if (dto.ItemId != null) bundle.ItemId = dto.ItemId.Value;
            if (dto.Quantity != null) bundle.Quantity = dto.Quantity.Value;

            await _skinAndCharacterBundleRepository.UpdateAsync(bundle);
        }

        public async Task DeleteSkinAndCharacterBundleAsync(int id) => await _skinAndCharacterBundleRepository.DeleteAsync(id);

        // ShopOrder & Details & TopUp (Read Only / Management)
        public async Task<IEnumerable<ShopOrderDto>> GetAllShopOrdersAsync(QueryParameters query)
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            var dtos = orders.Select(o => new ShopOrderDto
            {
                ShopOrderId = o.ShopOrderId,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                PlayerEmail = o.PlayerEmail,
                PlayerUserName = o.PlayerUserName,
                OrderCode = o.OrderCode,
                Currency = o.Currency ?? string.Empty
            });
            return dtos.ApplyQuery(query, (o, search) => o.UserId.ToString().Contains(search) || (o.PlayerUserName != null && o.PlayerUserName.Contains(search, StringComparison.OrdinalIgnoreCase)) || (o.PlayerEmail != null && o.PlayerEmail.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<ShopOrder?> GetShopOrderByIdAsync(int id)
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            return orders.FirstOrDefault(o => o.ShopOrderId == id);
        }

        public async Task<ShopOrder?> GetShopOrderByPaymentLinkIdAsync(string paymentLinkId)
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            return orders.FirstOrDefault(o => o.PaymentLinkId == paymentLinkId);
        }

        public async Task<ShopOrder?> GetShopOrderByOrderCodeAsync(long orderCode)
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            return orders.FirstOrDefault(o => o.OrderCode == orderCode);
        }

        public async Task UpdateShopOrderAsync(int id, ShopOrder updatedOrder)
        {
            var orders = await _shopOrderRepository.GetAllAsync();
            var order = orders.FirstOrDefault(o => o.ShopOrderId == id);
            
            if (order == null) throw new KeyNotFoundException("ShopOrder not found");

            // Basic order information
            order.TotalAmount = updatedOrder.TotalAmount;
            order.OrderDate = updatedOrder.OrderDate;
            
            // Customer information
            order.PlayerEmail = updatedOrder.PlayerEmail;
            order.PlayerUserName = updatedOrder.PlayerUserName;

            // Payment link related properties
            order.PaymentLinkId = updatedOrder.PaymentLinkId;
            order.QrCode = updatedOrder.QrCode;
            order.CheckoutUrl = updatedOrder.CheckoutUrl;
            order.Status = updatedOrder.Status;
            order.Currency = updatedOrder.Currency;

            // URLs
            order.ReturnUrl = updatedOrder.ReturnUrl;
            order.CancelUrl = updatedOrder.CancelUrl;

            // Cancellation
            order.CancellationReason = updatedOrder.CancellationReason;
            order.LastTransactionUpdate = updatedOrder.LastTransactionUpdate;

            await _shopOrderRepository.UpdateAsync(order);
        }

        public async Task<IEnumerable<ShopOrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId, QueryParameters query)
        {
            var details = await _shopOrderDetailRepository.GetAllAsync();
            var dtos = details.Where(d => d.ShopOrderId == orderId).Select(d => new ShopOrderDetailDto
            {
                ShopOrderDetailId = d.ShopOrderDetailId,
                ShopOrderId = d.ShopOrderId,
                SkinAndCharacterBundleId = d.SkinAndCharacterBundleId,
                GemBundleId = d.GemBundleId,
                ItemId = d.ItemId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            });
            return dtos.ApplyQuery(query, (d, search) => d.ItemId.ToString().Contains(search));
        }

        public async Task<IEnumerable<TopUpHistoryDto>> GetAllTopUpHistoriesAsync(QueryParameters query)
        {
            var histories = await _topUpHistoryRepository.GetAllAsync();
            var dtos = histories.Select(h => new TopUpHistoryDto
            {
                TopUpId = h.TopUpId,
                UserId = h.UserId,
                GemBundleId = h.GemBundleId,
                GemsAmount = h.GemsAmount,
                RealMoneyAmount = h.RealMoneyAmount,
                CurrencyCode = h.CurrencyCode,
                PaymentGateway = h.PaymentGateway,
                TransactionId = h.TransactionId,
                Status = h.Status,
                Date = h.Date
            });
            return dtos.ApplyQuery(query, (h, search) => h.UserId.ToString().Contains(search) || (h.TransactionId != null && h.TransactionId.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task PurchaseGemBundleAsync(int userId, int bundleId)
        {
            var bundle = await _gemBundleRepository.GetByIdAsync(bundleId);
            if (bundle == null) throw new KeyNotFoundException("Gem bundle not found");

            // Create ShopOrder for tracking the transaction natively
            var order = new ShopOrder
            {
                UserId = userId,
                TotalAmount = bundle.BundlePrice,
                OrderDate = DateTimeOffset.UtcNow,
                Status = PayOS.Models.V2.PaymentRequests.PaymentLinkStatus.Pending,
                Currency = "VND"
            };
            var createdOrder = await _shopOrderRepository.AddAsync(order);

            // Create ShopOrderDetail mapping to the GemBundle structure
            var detail = new ShopOrderDetail
            {
                ShopOrderId = createdOrder.ShopOrderId,
                GemBundleId = bundleId,
                ItemId = bundle.ItemId,
                Quantity = bundle.Quantity,
                UnitPrice = bundle.BundlePrice
            };
            await _shopOrderDetailRepository.AddAsync(detail);

            // Create TopUpHistory
            var history = new TopUpHistory
            {
                UserId = userId,
                GemBundleId = bundleId,
                GemsAmount = bundle.Quantity, 
                RealMoneyAmount = bundle.BundlePrice,
                CurrencyCode = "USD",
                PaymentGateway = "Simulator",
                TransactionId = Guid.NewGuid().ToString(),
                Status = "Success",
                Date = DateTime.UtcNow
            };
            await _topUpHistoryRepository.AddAsync(history);

            // Add to UserBundle
            var userBundle = new UserBundle
            {
                UserId = userId,
                GemBundleId = bundleId,
                Remaining = 1
            };
            await _userBundleRepository.AddAsync(userBundle);
        }

        public async Task PurchaseSkinBundleAsync(int userId, int bundleId)
        {
            var bundle = await _skinAndCharacterBundleRepository.GetByIdAsync(bundleId);
            if (bundle == null) throw new KeyNotFoundException("Skin bundle not found");

            // Create ShopOrder
            var order = new ShopOrder
            {
                UserId = userId,
                TotalAmount = bundle.BundlePrice,
                OrderDate = DateTimeOffset.UtcNow,
                Status = PayOS.Models.V2.PaymentRequests.PaymentLinkStatus.Pending,
                Currency = "VND"
            };
            var createdOrder = await _shopOrderRepository.AddAsync(order);

            // Create ShopOrderDetail
            var detail = new ShopOrderDetail
            {
                ShopOrderId = createdOrder.ShopOrderId,
                SkinAndCharacterBundleId = bundleId,
                Quantity = 1,
                UnitPrice = bundle.BundlePrice
            };
            await _shopOrderDetailRepository.AddAsync(detail);

            // Add to UserBundle
            var userBundle = new UserBundle
            {
                UserId = userId,
                SkinAndCharacterBundleId = bundleId,
                Remaining = 1
            };
            await _userBundleRepository.AddAsync(userBundle);

            // Add items from bundle to UserItem
            var userItem = new UserItem
            {
                UserId = userId,
                ItemId = bundle.ItemId,
                Quantity = bundle.Quantity,
                ShopOrderId = createdOrder.ShopOrderId
            };
            await _userItemRepository.AddAsync(userItem);
        }

        public async Task<ShopOrder> CreateShopOrderAsync(ShopOrder order)
        {
            var detailsToInsert = order.OrderDetails?.ToList() ?? new List<ShopOrderDetail>();
            order.OrderDetails = new List<ShopOrderDetail>();

            var createdOrder = await _shopOrderRepository.AddAsync(order);
            
            if (detailsToInsert.Any())
            {
                foreach (var detail in detailsToInsert)
                {
                    detail.ShopOrderId = createdOrder.ShopOrderId;
                    await _shopOrderDetailRepository.AddAsync(detail);
                }
            }

            createdOrder.OrderDetails = detailsToInsert;
            return createdOrder;
        }

        public async Task<UserItem?> GetUserItemAsync(int userId, int itemId)
        {
            var items = await _userItemRepository.GetAllAsync();
            return items.FirstOrDefault(ui => ui.UserId == userId && ui.ItemId == itemId);
        }

        public async Task CreateUserItemAsync(UserItem userItem)
        {
            await _userItemRepository.AddAsync(userItem);
        }

        public async Task UpdateUserItemAsync(UserItem userItem)
        {
            await _userItemRepository.UpdateAsync(userItem);
        }

        public async Task ProcessSuccessfulOrderAsync(ShopOrder order)
        {
            if (order == null) return;

            var allDetails = await _shopOrderDetailRepository.GetAllAsync();
            var details = allDetails.Where(d => d.ShopOrderId == order.ShopOrderId).ToList();

            foreach (var detail in details)
            {
                if (detail.GemBundleId.HasValue)
                {
                    var bundle = await _gemBundleRepository.GetByIdAsync(detail.GemBundleId.Value);
                    if (bundle == null) continue;

                    int totalGemsToAdd = bundle.Quantity; 

                    var userInventory = await _userItemRepository.GetByUserIdAsync(order.UserId);
                    var existingGems = userInventory.FirstOrDefault(ui => ui.ItemId == 4);

                    if (existingGems != null)
                    {
                        existingGems.Quantity += totalGemsToAdd;
                        existingGems.ShopOrderId = order.ShopOrderId;
                        await _userItemRepository.UpdateAsync(existingGems);
                    }
                    else
                    {
                        await _userItemRepository.AddAsync(new UserItem
                        {
                            UserId = order.UserId,
                            ItemId = bundle.ItemId,
                            Quantity = totalGemsToAdd,
                            ShopOrderId = order.ShopOrderId
                        });
                    }
                }
            }
        }
    }
}
