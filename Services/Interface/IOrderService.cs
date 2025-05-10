using BookHaven.DTOs;

namespace BookHaven.Services.Interface { 
    public interface IOrderService
{
    Task<int> PlaceOrderAsync(string userId, PlaceOrderRequest request);
    Task<bool> CancelOrderAsync(string userId, int orderId);
}
}
