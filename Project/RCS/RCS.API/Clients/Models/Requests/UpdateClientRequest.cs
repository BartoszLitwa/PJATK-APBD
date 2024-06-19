namespace RCS.API.Clients.Models.Requests;

public record UpdateClientRequest(int Id, string FirstName, string LastName, string Address, string Email, string PhoneNumber);