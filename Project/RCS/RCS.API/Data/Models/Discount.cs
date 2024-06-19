using System.ComponentModel.DataAnnotations;

namespace RCS.API.Data.Models;

public class Discount
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Value { get; set; } // Percentage

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}