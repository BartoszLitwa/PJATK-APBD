using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolos.API.Data.Models;

public class Subscription
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdSubscription { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required, AllowedValues(new int[] {1,3,6})]
    public int RenewallPeriod { get; set; }

    public DateTime EndTime { get; set; }
    public decimal Price { get; set; }
    
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}