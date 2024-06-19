using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS.API.Data.Models;

public class Contract
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }

    [Required]
    public int SoftwareId { get; set; }

    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public bool IsSigned { get; set; } = false;

    public List<Payment> Payments { get; set; } = new List<Payment>();
}