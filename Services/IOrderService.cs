using ShoeStore.Models;

namespace ShoeStore.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> GetOrderByNumberAsync(string orderNumber);
        Task<Order> CreateOrderFromCartAsync(int userId, Order orderDetails);
        Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<bool> UpdateTrackingNumberAsync(int id, string trackingNumber);
        Task<bool> CancelOrderAsync(int id);
        Task<string> GenerateOrderNumberAsync();
    }
}