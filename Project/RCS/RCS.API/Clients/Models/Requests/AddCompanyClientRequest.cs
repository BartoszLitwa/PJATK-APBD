namespace RCS.API.Clients.Models.Requests;

public record AddCompanyClientRequest(string CompanyName, string Address, string Email, string PhoneNumber, string KRS);