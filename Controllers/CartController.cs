using Microsoft.AspNetCore.Mvc;
using ShoeStore.Models;
using ShoeStore.Services;

namespace ShoeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(int userId)
        {
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return Ok(cartItems);
        }

        [HttpPost("{userId}/add")]
        public async Task<ActionResult<CartItem>> AddToCart(int userId, [FromBody] AddToCartRequest request)
        {
            if (request.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            try
            {
                var cartItem = await _cartService.AddToCartAsync(userId, request.ShoeId, request.Quantity);
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding to cart: {ex.Message}");
            }
        }

        [HttpPut("{userId}/update")]
        public async Task<IActionResult> UpdateCartItem(int userId, [FromBody] UpdateCartItemRequest request)
        {
            if (request.Quantity < 0)
            {
                return BadRequest("Quantity cannot be negative.");
            }

            var updated = await _cartService.UpdateCartItemQuantityAsync(userId, request.ShoeId, request.Quantity);
            
            if (!updated)
            {
                return NotFound("Cart item not found.");
            }

            return NoContent();
        }

        [HttpDelete("{userId}/remove/{shoeId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int shoeId)
        {
            var removed = await _cartService.RemoveFromCartAsync(userId, shoeId);
            
            if (!removed)
            {
                return NotFound("Cart item not found.");
            }

            return NoContent();
        }

        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpGet("{userId}/total")]
        public async Task<ActionResult<decimal>> GetCartTotal(int userId)
        {
            var total = await _cartService.GetCartTotalAsync(userId);
            return Ok(new { total });
        }

        [HttpGet("{userId}/count")]
        public async Task<ActionResult<int>> GetCartItemCount(int userId)
        {
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(new { count });
        }
    }

    public class AddToCartRequest
    {
        public int ShoeId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemRequest
    {
        public int ShoeId { get; set; }
        public int Quantity { get; set; }
    }
}