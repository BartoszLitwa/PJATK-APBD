using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trips.API.Data.Models;

[Table("Trip", Schema = "trip")]
public partial class Trip
{
    [Key]
    public int IdTrip { get; set; }

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [StringLength(220)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }

    [InverseProperty("Trip")]
    public virtual ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();

    [ForeignKey("IdTrip")]
    [InverseProperty("Trips")]
    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
