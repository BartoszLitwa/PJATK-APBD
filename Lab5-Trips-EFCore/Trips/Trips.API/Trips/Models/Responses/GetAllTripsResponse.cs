namespace Trips.API.Trips.Models.Responses;

public class GetAllTripsResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public IEnumerable<CountryResponse> Countries { get; set; }
    public IEnumerable<ClientResponse> Clients { get; set; }
}

public record CountryResponse
{
    public string Name { get; set; }
}

public record ClientResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}