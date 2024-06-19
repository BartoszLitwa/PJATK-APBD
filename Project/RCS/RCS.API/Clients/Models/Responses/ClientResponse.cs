namespace RCS.API.Clients.Models.Responses;

public record ClientResponse(int Id, string FirstName, string LastName, string Address, string Email, string PhoneNumber, string PESEL, bool IsDeleted);