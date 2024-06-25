using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS.API.Data.Models;

public class SubscriptionPayment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SubscriptionId { get; set; }

    [ForeignKey("SubscriptionId")]
    public Subscription Subscription { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }
}