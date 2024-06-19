namespace RCS.API.Clients.Models.Requests;

public record AddClientRequest(string FirstName, string LastName, string Address, string Email, string PhoneNumber, string PESEL);