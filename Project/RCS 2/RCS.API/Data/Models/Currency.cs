using System.ComponentModel.DataAnnotations;

namespace RCS.API.Data.Models;

public class Currency
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(3)]
    public string Code { get; set; }

    [Required]
    public decimal ExchangeRateToPLN { get; set; } 
}
