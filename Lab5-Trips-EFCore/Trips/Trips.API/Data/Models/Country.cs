using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trips.API.Data.Models;

[Table("Country", Schema = "trip")]
public partial class Country
{
    [Key]
    public int IdCountry { get; set; }

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [ForeignKey("IdCountry")]
    [InverseProperty("Countries")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
