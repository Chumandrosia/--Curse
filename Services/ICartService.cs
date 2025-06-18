using ShoeStore.Models;

namespace ShoeStore.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int shoeId);
        Task<CartItem> AddToCartAsync(int userId, int shoeId, int quantity = 1);
        Task<bool> UpdateCartItemQuantityAsync(int userId, int shoeId, int quantity);
        Task<bool> RemoveFromCartAsync(int userId, int shoeId);
        Task<bool> ClearCartAsync(int userId);
        Task<decimal> GetCartTotalAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
    }
}