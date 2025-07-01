using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Models
{
    public class ClassifiedItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(120)]
        public string Title { get; set; } = default!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid SellerId { get; set; }

        [Required, EmailAddress]
        public string SellerEmail { get; set; } = default!;

        [StringLength(50)]
        public string? Category { get; set; }

        public bool IsSold { get; set; } = false;
    }
}
