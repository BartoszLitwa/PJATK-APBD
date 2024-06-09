using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolos.API.Data.Models;

public class Sale
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdSale { get; set; }
    [ForeignKey("Client")]
    public int IdClient { get; set; }
    [ForeignKey("Subscription")] 
    public int IdSubscription { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public virtual Client Client { get; set; }
    public virtual Subscription Subscription { get; set; }
}