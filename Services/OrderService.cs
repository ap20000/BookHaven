using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> PlaceOrderAsync(string userId, PlaceOrderRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) throw new Exception("User not found");

        var order = new Order
        {
            ApplicationUserId = userId,
            OrderDate = DateTime.Now,
            PickupStatus = "Pending",
            ClaimCode = GenerateClaimCode(),
            DiscountApplied = false,
            DiscountPercent = CalculateDiscount(userId),
            TotalAmount = CalculateTotalAmount(request.OrderItems),
            OrderItems = request.OrderItems.Select(item => new OrderItem
            {
                BookID = item.BookID,
                Quantity = item.Quantity,
                PriceAtPurchase = item.PriceAtPurchase
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order.OrderID;
    }

    public async Task<bool> CancelOrderAsync(string userId, int orderId)
    {
        var order = await _context.Orders
            .Where(o => o.OrderID == orderId && o.ApplicationUserId == userId)
            .FirstOrDefaultAsync();

        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    private string GenerateClaimCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }

    private decimal CalculateDiscount(string userId)
    {
        // Reward for 5+ books: 5% discount
        var orderCount = _context.Orders.Count(o => o.ApplicationUserId == userId);
        if (orderCount >= 10)
        {
            return 0.10m; // 10% discount for 10+ orders
        }
        else if (orderCount >= 5)
        {
            return 0.05m; // 5% discount for 5+ orders
        }
        return 0.00m;
    }

    private decimal CalculateTotalAmount(List<OrderItemRequest> items)
    {
        decimal totalAmount = 0;
        foreach (var item in items)
        {
            totalAmount += item.PriceAtPurchase * item.Quantity;
        }
        return totalAmount;
    }
}
