using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ShoeId { get; set; }
        
        [Required]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User User { get; set; } = null!;
        public Shoe Shoe { get; set; } = null!;
    }
}