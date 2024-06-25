using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS.API.Data.Models;

public class Subscription
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey("ClientId")]
    public Client Client { get; set; }

    [Required]
    public int SoftwareId { get; set; }

    [ForeignKey("SoftwareId")]
    public Software Software { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public string RenewalPeriod { get; set; } // e.g., "monthly", "yearly"

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime NextRenewalDate { get; set; }

    public bool IsActive { get; set; } = true;

    public List<SubscriptionPayment> Payments { get; set; } = new List<SubscriptionPayment>();
}