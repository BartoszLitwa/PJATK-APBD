﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trips.API.Data.Models;

[Table("Client", Schema = "trip")]
public partial class Client
{
    [Key]
    public int IdClient { get; set; }

    [StringLength(120)]
    public string FirstName { get; set; } = null!;

    [StringLength(120)]
    public string LastName { get; set; } = null!;

    [StringLength(120)]
    public string Email { get; set; } = null!;

    [StringLength(120)]
    public string Telephone { get; set; } = null!;

    [StringLength(120)]
    public string Pesel { get; set; } = null!;

    [InverseProperty("Client")]
    public virtual ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();
}
