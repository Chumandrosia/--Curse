using Microsoft.AspNetCore.Mvc;
using ShoeStore.Models;
using ShoeStore.Services;

namespace ShoeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(order);
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<Order>> GetOrderByNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            
            if (order == null)
            {
                return NotFound($"Order with number {orderNumber} not found.");
            }

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPost("create/{userId}")]
        public async Task<ActionResult<Order>> CreateOrder(int userId, [FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var orderDetails = new Order
                {
                    ShippingAddress = request.ShippingAddress,
                    ShippingCity = request.ShippingCity,
                    ShippingPostalCode = request.ShippingPostalCode,
                    ShippingCountry = request.ShippingCountry
                };

                var order = await _orderService.CreateOrderFromCartAsync(userId, orderDetails);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating order: {ex.Message}");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var updated = await _orderService.UpdateOrderStatusAsync(id, request.Status);
            
            if (!updated)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpPatch("{id}/tracking")]
        public async Task<IActionResult> UpdateTrackingNumber(int id, [FromBody] UpdateTrackingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TrackingNumber))
            {
                return BadRequest("Tracking number cannot be empty.");
            }

            var updated = await _orderService.UpdateTrackingNumberAsync(id, request.TrackingNumber);
            
            if (!updated)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var cancelled = await _orderService.CancelOrderAsync(id);
            
            if (!cancelled)
            {
                return BadRequest("Order cannot be cancelled (not found or already delivered/cancelled).");
            }

            return NoContent();
        }
    }

    public class CreateOrderRequest
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingPostalCode { get; set; } = string.Empty;
        public string? ShippingCountry { get; set; } = "Україна";
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }

    public class UpdateTrackingRequest
    {
        public string TrackingNumber { get; set; } = string.Empty;
    }
}