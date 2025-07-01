using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.ViewModels;

public class ClassifiedItemCreateDto
{
    [Required, StringLength(120)]
    public string Title { get; set; } = default!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }
}