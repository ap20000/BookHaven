using BookHaven.Dtos;
using BookHaven.DTOs;

namespace BookHaven.Services.Interface { 
    public interface IOrderService
{
    Task<Order> PlaceOrderAsync(string userId, PlaceOrderRequest request);
    Task<bool> CancelOrderAsync(string userId, int orderId);
    Task<List<OrderSummaryDto>> GetOrdersByUserAsync(string userId);
    }
}
