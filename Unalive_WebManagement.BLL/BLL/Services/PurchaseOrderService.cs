using System.Collections.Generic;
using System.Threading.Tasks;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DAL.Repositories;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISkinAndCharacterBundleRepository _skinAndCharacterBundleRepository;
        private readonly IUserItemRepository _userItemRepository;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, ISkinAndCharacterBundleRepository skinAndCharacterBundleRepository, IUserItemRepository userItemRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _skinAndCharacterBundleRepository = skinAndCharacterBundleRepository;
            _userItemRepository = userItemRepository;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            return await _purchaseOrderRepository.GetAllWithDetailsAsync();
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id)
        {
            return await _purchaseOrderRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder order)
        {
            return await _purchaseOrderRepository.AddAsync(order);
        }

        public async Task UpdatePurchaseOrderAsync(int id, PurchaseOrder updatedOrder)
        {
            var existingOrder = await _purchaseOrderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException("PurchaseOrder not found");

            existingOrder.UserId = updatedOrder.UserId;
            existingOrder.SkinAndCharacterBundleId = updatedOrder.SkinAndCharacterBundleId;
            existingOrder.GemCost = updatedOrder.GemCost;
            existingOrder.Status = updatedOrder.Status;

            await _purchaseOrderRepository.UpdateAsync(existingOrder);
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            await _purchaseOrderRepository.DeleteAsync(id);
        }

        public async Task<PurchaseOrder> ProcessBundlePurchaseAsync(int userId, int bundleId)
        {
            // 1. Fetch the bundle
            var bundle = await _skinAndCharacterBundleRepository.GetByIdAsync(bundleId);
            if (bundle == null || bundle.Status != SkinAndCharacterBundle.StatusEnum.Available)
            {
                throw new Exception("Bundle is not available.");
            }

            // 2. Fetch user's inventory
            var userInventory = (await _userItemRepository.GetByUserIdAsync(userId)).ToList();

            // check if item already owned
            var alreadyOwnsItem = userInventory.Any(i => i.ItemId == bundle.ItemId);
            if (alreadyOwnsItem)
            {
                throw new Exception("This item is already owned by the user.");
            }

            // 3. Check Gem Balance
            var gemItem = userInventory.FirstOrDefault(i => i.ItemId == 4);
            int costInGems = (int)bundle.BundlePrice;

            if (gemItem == null || gemItem.Quantity < costInGems)
            {
                throw new Exception("Not enough gems to purchase this bundle.");
            }

            // 4. Deduct the Gems
            gemItem.Quantity -= costInGems;
            await _userItemRepository.UpdateAsync(gemItem);

            // 5. Create the Purchase Order
            var newOrder = new PurchaseOrder
            {
                UserId = userId,
                SkinAndCharacterBundleId = bundleId,
                GemCost = bundle.BundlePrice,
                Status = InGamePurchaseStatus.Completed,
                PurchaseDate = DateTimeOffset.UtcNow
            };

            var savedOrder = await _purchaseOrderRepository.AddAsync(newOrder);

            // 6. GRANT THE ITEM TO THE PLAYER
            var existingItem = userInventory.FirstOrDefault(i => i.ItemId == bundle.ItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += bundle.Quantity;
                await _userItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItemToGrant = new UserItem
                {
                    UserId = userId,
                    ItemId = bundle.ItemId,
                    Quantity = bundle.Quantity,
                    ShopOrderId = null,
                    PurchaseOrderId = savedOrder.PurchaseOrderId
                };

                await _userItemRepository.AddAsync(newItemToGrant);
            }

            return savedOrder;
        }

        // Refund Logic incase of a refund request
        public async Task<PurchaseOrder> RefundPurchaseAsync(int purchaseOrderId)
        {
            // 1. Find the existing order
            var order = await _purchaseOrderRepository.GetByIdWithDetailsAsync(purchaseOrderId);
            if (order == null) throw new KeyNotFoundException("Order not found.");

            // 2. Prevent double-refunding
            if (order.Status == InGamePurchaseStatus.Refunded)
            {
                throw new InvalidOperationException("This order has already been refunded.");
            }

            if (order.Status != InGamePurchaseStatus.Completed)
            {
                throw new InvalidOperationException("Only completed orders can be refunded.");
            }

            // 3. Return the Gems
            var userInventory = await _userItemRepository.GetByUserIdAsync(order.UserId);
            var gemItem = userInventory.FirstOrDefault(i => i.ItemId == 4);

            if (gemItem != null)
            {
                // Add the historical cost back to their inventory
                gemItem.Quantity += (int)order.GemCost;
                await _userItemRepository.UpdateAsync(gemItem);
            }

            // 4. Update the order status
            order.Status = InGamePurchaseStatus.Refunded;
            await _purchaseOrderRepository.UpdateAsync(order);

            // TODO: In a complete system, you must also remove the bundle items 
            // (skins/characters) from the user's inventory here!

            return order;
        }
    }
}