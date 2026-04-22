using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class OrderController(
        [FromKeyedServices("OrderClient")] PayOSClient client,
        IShopService shopService,
        IOrderTransactionService orderTransactionService,
        IUserService userService) : ControllerBase
    {
        private readonly PayOSClient _client = client;
        private readonly IShopService _shopService = shopService;
        private readonly IOrderTransactionService _orderTransactionService = orderTransactionService;
        private readonly IUserService _userService = userService;

        [HttpGet("{id}")]
        public async Task<ActionResult<ShopOrder>> Get(int id)
        {
            var order = await _shopService.GetShopOrderByIdAsync(id);
            if (order == null || string.IsNullOrEmpty(order.PaymentLinkId))
            {
                return NotFound();
            }

            try
            {
                var paymentLink = await _client.PaymentRequests.GetAsync(order.PaymentLinkId);

                bool justPaid = order.Status != PaymentLinkStatus.Paid && paymentLink.Status == PaymentLinkStatus.Paid;
                
                order.Status = paymentLink.Status;
                order.Amount = paymentLink.Amount;
                order.AmountPaid = paymentLink.AmountPaid;
                order.AmountRemaining = paymentLink.AmountRemaining;
                order.CreatedAt = DateTimeOffset.TryParse(paymentLink.CreatedAt, out var createdAt) ? createdAt : null;
                order.CanceledAt = DateTimeOffset.TryParse(paymentLink.CanceledAt, out var canceledAt) ? canceledAt : null;
                order.CancellationReason = paymentLink.CancellationReason;

                if (paymentLink.Transactions != null && paymentLink.Transactions.Count > 0)
                {
                    await _orderTransactionService.DeleteTransactionsByOrderIdAsync(order.ShopOrderId);

                    var transactions = paymentLink.Transactions.Select(t => new OrderTransaction
                    {
                        OrderId = order.ShopOrderId,
                        OrderCode = order.OrderCode,
                        PaymentLinkId = order.PaymentLinkId,
                        Reference = t.Reference,
                        Amount = t.Amount,
                        AccountNumber = t.AccountNumber,
                        Description = t.Description,
                        TransactionDateTime = DateTimeOffset.TryParse(t.TransactionDateTime, out var transactionDateTime) ? transactionDateTime : DateTimeOffset.Now,
                        VirtualAccountName = t.VirtualAccountName,
                        VirtualAccountNumber = t.VirtualAccountNumber,
                        CounterAccountBankId = t.CounterAccountBankId,
                        CounterAccountBankName = t.CounterAccountBankName,
                        CounterAccountName = t.CounterAccountName,
                        CounterAccountNumber = t.CounterAccountNumber
                    }).ToList();

                    await _orderTransactionService.CreateTransactionsAsync(transactions);
                    order.LastTransactionUpdate = DateTimeOffset.Now;
                }

                await _shopService.UpdateShopOrderAsync(order.ShopOrderId, order);
                
                // Incase webhook has problem, use this
                //if (justPaid)
                //{
                //    await _shopService.ProcessSuccessfulOrderAsync(order.ShopOrderId);
                //}

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Failed to retrieve order {id}", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShopOrder>> CreatePayment([FromBody] OrderCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Order data is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.UserId > 0)
            {
                var user = await _userService.GetUserByIdAsync(request.UserId);
                if (user != null)
                {
                    request.PlayerEmail = user.Email;
                    request.PlayerUserName = user.UserName; 
                }
            }

            var orderCode = DateTimeOffset.Now.ToUnixTimeSeconds();
            var callbackReturnUrl = request.ReturnUrl ?? "https://your-domain.com/success";
            var callbackCancelUrl = request.CancelUrl ?? "https://your-domain.com/cancel";
            
            var expirationTime = DateTimeOffset.Now.AddMinutes(15);

            try
            {
                float calculatedTotalAmount = 0;
                var paymentItems = new List<PaymentLinkItem>();
                var detailsToInsert = new List<ShopOrderDetail>();

                foreach (var itemRequest in request.Items)
                {
                    var bundle = await _shopService.GetGemBundleByIdAsync(itemRequest.BundleId);
                    
                    if (bundle == null)
                    {
                        return BadRequest(new { message = $"Bundle with ID {itemRequest.BundleId} not found." });
                    }

                    int bundleBuyQuantity = 1;
                    float actualPrice = bundle.BundlePrice;
                    string bundleName = bundle.BundleName;
                    
                    calculatedTotalAmount += (actualPrice * bundleBuyQuantity);

                    paymentItems.Add(new PaymentLinkItem
                    {
                        Name = bundleName,
                        Quantity = bundleBuyQuantity,
                        Price = (int)Math.Round(actualPrice)
                    });

                    detailsToInsert.Add(new ShopOrderDetail
                    {
                        Quantity = bundleBuyQuantity,
                        UnitPrice = actualPrice,
                        GemBundleId = itemRequest.BundleId,
                        ItemId = 4
                    });
                }

                int paymentAmount = (int)Math.Round(calculatedTotalAmount);

                var paymentRequest = new CreatePaymentLinkRequest
                {
                    OrderCode = orderCode,
                    Amount = paymentAmount,
                    Description = $"Order {orderCode}",
                    ReturnUrl = callbackReturnUrl,
                    CancelUrl = callbackCancelUrl,
                    BuyerEmail = request.PlayerEmail,
                    BuyerName = request.PlayerUserName,
                    ExpiredAt = expirationTime.ToUnixTimeSeconds(),
                    Items = paymentItems
                };

                var paymentResponse = await _client.PaymentRequests.CreateAsync(paymentRequest);

                var order = new ShopOrder
                {
                    UserId = request.UserId > 0 ? request.UserId : 1,
                    PlayerEmail = request.PlayerEmail,
                    PlayerUserName = request.PlayerUserName,
                    TotalAmount = calculatedTotalAmount,
                    OrderCode = orderCode,
                    OrderDate = DateTimeOffset.Now,
                    PaymentLinkId = paymentResponse.PaymentLinkId,
                    QrCode = paymentResponse.QrCode,
                    CheckoutUrl = paymentResponse.CheckoutUrl,
                    Status = paymentResponse.Status,
                    Amount = paymentResponse.Amount,
                    AmountPaid = 0,
                    AmountRemaining = paymentResponse.Amount,
                    Bin = paymentResponse.Bin,
                    AccountNumber = paymentResponse.AccountNumber,
                    AccountName = paymentResponse.AccountName,
                    Currency = paymentResponse.Currency,
                    ReturnUrl = callbackReturnUrl,
                    CancelUrl = callbackCancelUrl,
                    CreatedAt = DateTimeOffset.Now,
                    ExpiredAt = expirationTime,
                    OrderDetails = detailsToInsert 
                };

                var createdOrder = await _shopService.CreateShopOrderAsync(order);

                return CreatedAtAction(nameof(Get), new { id = createdOrder.ShopOrderId }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create order", error = ex.Message });
            }
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<ActionResult<PaymentLink>> CancelPayment(int orderId, [FromQuery] string cancellationReason)
        {
            var order = await _shopService.GetShopOrderByIdAsync(orderId);
            if (order == null || string.IsNullOrEmpty(order.PaymentLinkId))
            {
                return NotFound();
            }

            try
            {
                var paymentLink = await _client.PaymentRequests.CancelAsync(order.PaymentLinkId, cancellationReason ?? "Cancelled by user");

                order.Status = paymentLink.Status;
                order.Amount = paymentLink.Amount;
                order.AmountPaid = paymentLink.AmountPaid;
                order.AmountRemaining = paymentLink.AmountRemaining;
                order.CanceledAt = DateTimeOffset.TryParse(paymentLink.CanceledAt, out var canceledAt) ? canceledAt : null;
                order.CancellationReason = paymentLink.CancellationReason;

                if (paymentLink.Transactions != null && paymentLink.Transactions.Count > 0)
                {
                    await _orderTransactionService.DeleteTransactionsByOrderIdAsync(order.ShopOrderId);

                    var transactions = paymentLink.Transactions.Select(t => new OrderTransaction
                    {
                        OrderId = order.ShopOrderId,
                        OrderCode = order.OrderCode,
                        PaymentLinkId = order.PaymentLinkId,
                        Reference = t.Reference,
                        Amount = t.Amount,
                        AccountNumber = t.AccountNumber,
                        Description = t.Description,
                        TransactionDateTime = DateTimeOffset.TryParse(t.TransactionDateTime, out var transactionDateTime) ? transactionDateTime : DateTimeOffset.Now,
                        VirtualAccountName = t.VirtualAccountName,
                        VirtualAccountNumber = t.VirtualAccountNumber,
                        CounterAccountBankId = t.CounterAccountBankId,
                        CounterAccountBankName = t.CounterAccountBankName,
                        CounterAccountName = t.CounterAccountName,
                        CounterAccountNumber = t.CounterAccountNumber
                    }).ToList();

                    await _orderTransactionService.CreateTransactionsAsync(transactions);
                    order.LastTransactionUpdate = DateTimeOffset.Now;
                }

                await _shopService.UpdateShopOrderAsync(order.ShopOrderId, order);
                return Ok(paymentLink);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Failed to cancel order {orderId}", error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShopOrderDto>>> GetAllOrders([FromQuery] QueryParameters query)
        {
            return Ok(await _shopService.GetAllShopOrdersAsync(query));
        }

        [HttpPost("debug/force-success/{orderId}")]
        public async Task<IActionResult> ForceSuccess(int orderId)
        {
            var order = await _shopService.GetShopOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");

            // Force the status to Paid in the DB
            order.Status = PaymentLinkStatus.Paid;
            await _shopService.UpdateShopOrderAsync(orderId, order);

            // Manually trigger the gem delivery
            await _shopService.ProcessSuccessfulOrderAsync(order);

            return Ok("Gems processed. Check the UserItem table for UserId: " + order.UserId);
        }
    }
}
