using System.Collections.Generic;
using System.Threading.Tasks;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
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
    }
}