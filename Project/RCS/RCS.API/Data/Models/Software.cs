using System.ComponentModel.DataAnnotations;

namespace RCS.API.Data.Models;

public class Software
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string CurrentVersion { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; } // For upfront cost

    public List<Discount> Discounts { get; set; } = new List<Discount>();
}
