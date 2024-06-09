using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolos.API.Data.Models;

public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPayment { get; set; }

    public DateTime Date { get; set; }
    [ForeignKey("Client")]
    public int IdClient { get; set; }
    [ForeignKey("Subscription")] 
    public int IdSubscription { get; set; }
    
    public virtual Client Client { get; set; }
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}