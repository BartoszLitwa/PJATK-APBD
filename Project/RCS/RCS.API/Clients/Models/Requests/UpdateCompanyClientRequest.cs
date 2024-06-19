namespace RCS.API.Clients.Models.Requests;

public record UpdateCompanyClientRequest(int Id, string CompanyName, string Address, string Email, string PhoneNumber);