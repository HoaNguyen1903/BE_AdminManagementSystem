using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class OrderTransactionService : IOrderTransactionService
    {
        private readonly IOrderTransactionRepository _transactionRepository;

        public OrderTransactionService(IOrderTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<OrderTransaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<OrderTransaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<OrderTransaction>> GetTransactionsByOrderIdAsync(int orderId)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return transactions.Where(t => t.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderTransaction>> GetTransactionsByOrderCodeAsync(long orderCode)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return transactions.Where(t => t.OrderCode == orderCode);
        }

        public async Task<IEnumerable<OrderTransaction>> GetTransactionsByPaymentLinkIdAsync(string paymentLinkId)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return transactions.Where(t => t.PaymentLinkId == paymentLinkId);
        }

        public async Task<OrderTransaction> CreateTransactionAsync(OrderTransaction transaction)
        {
            return await _transactionRepository.AddAsync(transaction);
        }

        public async Task CreateTransactionsAsync(IEnumerable<OrderTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                await _transactionRepository.AddAsync(transaction);
            }
        }

        public async Task UpdateTransactionAsync(int id, OrderTransaction updatedTransaction)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) throw new KeyNotFoundException("OrderTransaction not found");

            transaction.OrderId = updatedTransaction.OrderId;
            transaction.OrderCode = updatedTransaction.OrderCode;
            transaction.PaymentLinkId = updatedTransaction.PaymentLinkId;
            transaction.Reference = updatedTransaction.Reference;
            transaction.Amount = updatedTransaction.Amount;
            transaction.AccountNumber = updatedTransaction.AccountNumber;
            transaction.Description = updatedTransaction.Description;
            transaction.TransactionDateTime = updatedTransaction.TransactionDateTime;
            transaction.VirtualAccountName = updatedTransaction.VirtualAccountName;
            transaction.VirtualAccountNumber = updatedTransaction.VirtualAccountNumber;
            transaction.CounterAccountBankId = updatedTransaction.CounterAccountBankId;
            transaction.CounterAccountBankName = updatedTransaction.CounterAccountBankName;
            transaction.CounterAccountName = updatedTransaction.CounterAccountName;
            transaction.CounterAccountNumber = updatedTransaction.CounterAccountNumber;

            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }

        public async Task DeleteTransactionsByOrderIdAsync(int orderId)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            var transactionsToRemove = transactions.Where(t => t.OrderId == orderId).ToList();

            foreach (var transaction in transactionsToRemove)
            {
                await _transactionRepository.DeleteAsync(transaction.Id);
            }
        }
    }
}