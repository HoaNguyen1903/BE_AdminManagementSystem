using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly PayOSClient _client;
        private readonly IShopService _shopService;
        private readonly IOrderTransactionService _orderTransactionService;

        public WebhookController(
            [FromKeyedServices("OrderClient")] PayOSClient client,
            IShopService shopService,
            IOrderTransactionService orderTransactionService)
        {
            _client = client;
            _shopService = shopService;
            _orderTransactionService = orderTransactionService;
        }

        private static DateTimeOffset ParseWebhookDateTime(string dateTimeString)
        {
            // Try to parse the standard ISO format first (2025-10-10T11:13:30+07:00)
            if (DateTimeOffset.TryParse(dateTimeString, out var result))
            {
                return result;
            }

            // Handle webhook format (2023-02-04 18:25:00)
            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dateTime))
            {
                return new DateTimeOffset(dateTime, TimeSpan.FromHours(7));
            }

            // Fallback to current time if parsing fails
            return DateTimeOffset.Now;
        }

        [HttpPost("payment")]
        public async Task<ActionResult> VerifyPayment(Webhook webhook)
        {
            if (webhook == null)
            {
                return BadRequest("Webhook data is required");
            }

            try
            {
                var webhookData = await _client.Webhooks.VerifyAsync(webhook);
                //if (webhookData.OrderCode == 123 && webhookData.Description == "VQRIO123" && webhookData.AccountNumber == "12345678")
                //{
                //    return Ok(new { message = "Webhook processed successfully" });
                //}

                var order = await _shopService.GetShopOrderByOrderCodeAsync(webhookData.OrderCode);

                if (order != null)
                {
                    var existingTransactions = await _orderTransactionService.GetTransactionsByOrderIdAsync(order.ShopOrderId);
                    var transactionExists = existingTransactions.Any(t => t.Reference == webhookData.Reference);

                    if (!transactionExists)
                    {
                        var transaction = new OrderTransaction
                        {
                            OrderId = order.ShopOrderId,
                            OrderCode = webhookData.OrderCode,
                            PaymentLinkId = order.PaymentLinkId ?? "",
                            Reference = webhookData.Reference,
                            Amount = webhookData.Amount,
                            AccountNumber = webhookData.AccountNumber,
                            Description = webhookData.Description,
                            TransactionDateTime = ParseWebhookDateTime(webhookData.TransactionDateTime),
                            VirtualAccountName = webhookData.VirtualAccountName,
                            VirtualAccountNumber = webhookData.VirtualAccountNumber,
                            CounterAccountBankId = webhookData.CounterAccountBankId,
                            CounterAccountBankName = webhookData.CounterAccountBankName,
                            CounterAccountName = webhookData.CounterAccountName,
                            CounterAccountNumber = webhookData.CounterAccountNumber
                        };

                        await _orderTransactionService.CreateTransactionAsync(transaction);
                    }

                    var allTransactions = await _orderTransactionService.GetTransactionsByOrderIdAsync(order.ShopOrderId);
                    var totalAmountPaid = allTransactions.Sum(t => t.Amount);

                    var amountRemaining = (decimal)order.Amount - totalAmountPaid;
                    order.Status = amountRemaining > 0 ? PaymentLinkStatus.Underpaid : PaymentLinkStatus.Paid;
                    order.LastTransactionUpdate = DateTimeOffset.Now;

                    await _shopService.UpdateShopOrderAsync(order.ShopOrderId, order);

                    if (order.Status == PaymentLinkStatus.Paid)
                    {
                        await _shopService.ProcessSuccessfulOrderAsync(order.ShopOrderId);
                    }
                }

                return Ok(new { message = "Webhook processed successfully", orderCode = webhookData.OrderCode });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook processing error: {ex.Message}");
                return Problem(ex.Message);
            }
        }
    }
}
