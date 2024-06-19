using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS.API.Data.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ContractId { get; set; }

    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }
}