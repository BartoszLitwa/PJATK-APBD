using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolos.API.Data.Models;

public class Discount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdDiscount { get; set; }
    public int Value { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    [ForeignKey("Client")]
    public int IdClient { get; set; }
    
    public virtual Client Client { get; set; }
}