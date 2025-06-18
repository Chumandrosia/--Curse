using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        // Shipping details
        [Required]
        [StringLength(255)]
        public string ShippingAddress { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ShippingCity { get; set; } = string.Empty;
        
        [Required]
        [StringLength(10)]
        public string ShippingPostalCode { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? ShippingCountry { get; set; } = "Україна";
        
        [StringLength(20)]
        public string? TrackingNumber { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
    
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }
}