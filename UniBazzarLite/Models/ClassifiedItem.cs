using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Models
{
    // Represents an item listed for sale in the classifieds
    public class ClassifiedItem
    {
        // Unique ID for the item
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(120)]
        public string Title { get; set; } = default!; // Item name

        [StringLength(2000)]
        public string? Description { get; set; } // Item details

        [Range(0.01, 100000)]
        public decimal Price { get; set; } // Price in local currency

        public DateTime PostedAt { get; set; } = DateTime.UtcNow; // When it was posted

        public Guid SellerId { get; set; } // Who is selling (user ID)
        public string? SellerEmail { get; set; } // Seller's email

        [StringLength(50)]
        public string? Category { get; set; } // Category (e.g., Electronics, Books)

        public bool IsSold { get; set; } = false; // Has it been sold?
    }
}
