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
        private readonly ApplicationDbContext _context;


        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("[ERROR] userId claim not found in token.");
                return Unauthorized("User identity not found.");
            }

            if (request == null || request.Items == null || !request.Items.Any())
            {
                Console.WriteLine("[ERROR] Invalid or empty order request.");
                return BadRequest("Order request is invalid.");
            }

            try
            {
                var orderId = await _orderService.PlaceOrderAsync(userId, request);

                Console.WriteLine($"[DEBUG] Order placed for userId {userId}. OrderId: {orderId}");

                return Ok(new
                {
                    OrderId = orderId,
                    Message = "Order placed successfully."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception while placing order: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while placing the order.");
            }
        }


        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("[ERROR] userId claim not found in token.");
                return Unauthorized("User identity not found.");
            }

            Console.WriteLine($"[DEBUG] CancelOrder requested. UserId: {userId}, OrderId: {orderId}");

            try
            {
                var result = await _orderService.CancelOrderAsync(userId, orderId);

                if (!result)
                {
                    Console.WriteLine($"[WARN] Cancellation failed for OrderId: {orderId} by UserId: {userId}");
                    return BadRequest("Order cannot be cancelled.");
                }

                Console.WriteLine($"[DEBUG] OrderId {orderId} cancelled by UserId: {userId}");
                return Ok("Order cancelled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception during cancellation: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
