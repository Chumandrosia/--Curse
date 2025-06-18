using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShoeStoreContext _context;
        private readonly ICartService _cartService;

        public OrderService(ShoeStoreContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Shoe)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Shoe)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Shoe)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Shoe)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order> CreateOrderFromCartAsync(int userId, Order orderDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Get cart items
                var cartItems = await _cartService.GetCartItemsAsync(userId);
                if (!cartItems.Any())
                    throw new InvalidOperationException("Cart is empty");

                // Calculate total
                var total = await _cartService.GetCartTotalAsync(userId);

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    OrderNumber = await GenerateOrderNumberAsync(),
                    TotalAmount = total,
                    Status = OrderStatus.Pending,
                    ShippingAddress = orderDetails.ShippingAddress,
                    ShippingCity = orderDetails.ShippingCity,
                    ShippingPostalCode = orderDetails.ShippingPostalCode,
                    ShippingCountry = orderDetails.ShippingCountry ?? "Україна",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order items
                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ShoeId = cartItem.ShoeId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Shoe.Price
                    };
                    
                    _context.OrderItems.Add(orderItem);

                    // Update stock
                    var shoe = await _context.Shoes.FindAsync(cartItem.ShoeId);
                    if (shoe != null)
                    {
                        shoe.StockQuantity -= cartItem.Quantity;
                        if (shoe.StockQuantity < 0)
                            throw new InvalidOperationException($"Insufficient stock for {shoe.Name}");
                    }
                }

                await _context.SaveChangesAsync();

                // Clear cart
                await _cartService.ClearCartAsync(userId);

                await transaction.CommitAsync();
                
                return await GetOrderByIdAsync(order.Id) ?? order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTrackingNumberAsync(int id, string trackingNumber)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            order.TrackingNumber = trackingNumber;
            order.UpdatedAt = DateTime.UtcNow;
            
            if (order.Status == OrderStatus.Confirmed || order.Status == OrderStatus.Processing)
            {
                order.Status = OrderStatus.Shipped;
            }
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Shoe)
                .FirstOrDefaultAsync(o => o.Id == id);
                
            if (order == null || order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
                return false;

            // Restore stock
            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.Shoe != null)
                {
                    orderItem.Shoe.StockQuantity += orderItem.Quantity;
                }
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            var orderNumber = $"ORD-{timestamp}-{random}";
            
            // Ensure uniqueness
            while (await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber))
            {
                random = new Random().Next(1000, 9999);
                orderNumber = $"ORD-{timestamp}-{random}";
            }
            
            return orderNumber;
        }
    }
}