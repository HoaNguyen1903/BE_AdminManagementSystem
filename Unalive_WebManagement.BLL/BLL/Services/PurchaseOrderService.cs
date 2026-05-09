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
            var bundle = await _skinAndCharacterBundleRepository.GetByIdAsync(bundleId);
            if (bundle == null)
            {
                throw new Exception("Bundle not found.");
            }

            var userInventory = await _userItemRepository.GetByUserIdAsync(userId);
            var gemItem = userInventory.FirstOrDefault(i => i.ItemId == 4);

            int costInGems = (int)bundle.BundlePrice;

            if (gemItem == null || gemItem.Quantity < costInGems)
            {
                throw new Exception("Not enough gems to purchase this bundle.");
            }

            gemItem.Quantity -= costInGems;
            await _userItemRepository.UpdateAsync(gemItem);

            var newOrder = new PurchaseOrder
            {
                UserId = userId,
                SkinAndCharacterBundleId = bundleId,
                GemCost = bundle.BundlePrice,
                Status = InGamePurchaseStatus.Completed,
                PurchaseDate = DateTimeOffset.UtcNow
            };

            return await _purchaseOrderRepository.AddAsync(newOrder);
        }
    }
}