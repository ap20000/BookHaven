
using BookHaven.Dtos;
using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> PlaceOrderAsync(string userId, PlaceOrderRequest request)
        {
            decimal total = 0;
            var orderItems = new List<OrderItem>();
            int totalQuantity = 0;

            foreach (var item in request.Items)
            {
                var book = await _context.Books.FindAsync(item.BookID);
                if (book == null) throw new Exception($"Book ID {item.BookID} not found.");

                decimal price = book.Price * item.Quantity;
                total += price;
                totalQuantity += item.Quantity;

                orderItems.Add(new OrderItem
                {
                    BookID = book.BookID,
                    Quantity = item.Quantity,
                    PriceAtPurchase = book.Price
                });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found.");

            // Determine discount
            int discountPercent = 0;

            if (totalQuantity >= 5)
            {
                discountPercent += 5; // 5% for buying 5 or more books
            }

            if (user.SuccessfulOrderCount >= 10)
            {
                discountPercent += 10; // 10% stackable for 10+ successful orders
            }

            var order = new Order
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.UtcNow,
                PickupStatus = "Pending",
                ClaimCode = Guid.NewGuid().ToString().Substring(0, 6).ToUpper(),
                DiscountApplied = discountPercent > 0,
                DiscountPercent = discountPercent,
                TotalAmount = total * (1 - (discountPercent / 100m)),
                Status = "Placed",
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.OrderID;
        }


        public async Task<bool> CancelOrderAsync(string userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderID == orderId && o.ApplicationUserId == userId);

            if (order == null || order.PickupStatus != "Pending") return false;

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<OrderSummaryDto>> GetOrdersByUserAsync(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.ApplicationUserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(order => new OrderSummaryDto
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PickupStatus = order.PickupStatus,
                DiscountApplied = order.DiscountApplied,
                DiscountPercent = order.DiscountPercent,
                ClaimCode = order.ClaimCode,
                Items = order.OrderItems.Select(oi => new Dtos.OrderItemDto
                {
                    BookID = oi.BookID,
                    Title = oi.Book.Title,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList()
            }).ToList();
        }
    }
}
