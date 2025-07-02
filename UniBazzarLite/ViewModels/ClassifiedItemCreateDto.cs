using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.ViewModels;

// DTO (Data Transfer Object) for creating a new classified item via the API
public class ClassifiedItemCreateDto
{
    [Required, StringLength(120)]
    public string Title { get; set; } = default!; // Item name

    [StringLength(2000)]
    public string? Description { get; set; } // Item details

    [Range(0.01, 100000)]
    public decimal Price { get; set; } // Price in local currency

    [StringLength(50)]
    public string? Category { get; set; } // Category (optional)
}