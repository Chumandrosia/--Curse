using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int ShoeId { get; set; }
        
        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; } // Price at the time of order
        
        // Navigation properties
        public Order Order { get; set; } = null!;
        public Shoe Shoe { get; set; } = null!;
    }
}