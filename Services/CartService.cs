using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Services
{
    public class CartService : ICartService
    {
        private readonly ShoeStoreContext _context;

        public CartService(ShoeStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Shoe)
                .Where(ci => ci.UserId == userId)
                .OrderBy(ci => ci.CreatedAt)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int shoeId)
        {
            return await _context.CartItems
                .Include(ci => ci.Shoe)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ShoeId == shoeId);
        }

        public async Task<CartItem> AddToCartAsync(int userId, int shoeId, int quantity = 1)
        {
            var existingItem = await GetCartItemAsync(userId, shoeId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _context.SaveChangesAsync();
                return existingItem;
            }

            var cartItem = new CartItem
            {
                UserId = userId,
                ShoeId = shoeId,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            
            return await GetCartItemAsync(userId, shoeId) ?? cartItem;
        }

        public async Task<bool> UpdateCartItemQuantityAsync(int userId, int shoeId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ShoeId == shoeId);
                
            if (cartItem == null)
                return false;

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int shoeId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ShoeId == shoeId);
                
            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return false;

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Shoe)
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity * ci.Shoe.Price);
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }
    }
}