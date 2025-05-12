using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Services;
using BookHaven.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookHaven.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public OrdersController(IOrderService orderService, IEmailService emailService, ApplicationDbContext context)
        {
            _orderService = orderService;
            _emailService = emailService;
            _context = context;
        }

        [HttpGet("claim/{claimCode}")]
        [Authorize(Roles = "staff,admin")]
        public async Task<IActionResult> GetOrderByClaimCode(string claimCode)
        {
            try
            {
                var order = await _orderService.GetOrderByClaimCodeAsync(claimCode);
                if (order == null)
                    return NotFound("No order found with this claim code.");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User identity not found.");

            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest("Order request is invalid.");

            try
            {
                // Step 1: Place the order and get full order object
                var order = await _orderService.PlaceOrderAsync(userId, request);

                // Step 2: Get user info from database
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    // Step 3: Format and send the email
                    var emailBody = $@"
                        Hi {user.FullName},

                        Thank you for placing your order at BookHaven!

                        🧾 Order ID: {order.OrderID}
                        🔐 Claim Code: {order.ClaimCode}
                        💵 Total: ${order.TotalAmount}

                        Please bring your claim code to the counter to collect your books.

                        Regards,  
                        BookHaven Team";

                    await _emailService.SendAsync(
                        user.Email,
                        "Your BookHaven Order Confirmation",
                        emailBody
                    );
                }

                // Step 4: Return success response with order info
                return Ok(new
                {
                    OrderId = order.OrderID,
                    ClaimCode = order.ClaimCode,
                    Message = "Order placed and confirmation email sent."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while placing the order.");
            }
        }

        [HttpPost("fulfill/{claimCode}")]
        [Authorize(Roles = "staff,admin")]
        public async Task<IActionResult> FulfillOrderByClaimCode(string claimCode)
        {
            if (string.IsNullOrWhiteSpace(claimCode))
                return BadRequest("Claim code is required.");

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.ClaimCode == claimCode && o.PickupStatus == "Pending");

            if (order == null)
                return NotFound("Invalid or already fulfilled claim code.");

            order.PickupStatus = "Fulfilled";
            order.Status = "Completed";
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Order with claim code {claimCode} fulfilled successfully." });
        }



        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User identity not found.");
            }

            try
            {
                var result = await _orderService.CancelOrderAsync(userId, orderId);

                if (!result)
                {
                    return BadRequest("Order cannot be cancelled.");
                }

                return Ok("Order cancelled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(string userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve orders: {ex.Message}");
            }
        }
    }
}
