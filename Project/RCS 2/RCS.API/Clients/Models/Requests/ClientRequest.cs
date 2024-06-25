namespace RCS.API.Clients.Models.Requests;

public abstract record ClientRequest(
    string Address,
    string Email,
    string PhoneNumber);

public record IndividualClientRequest(
    string FirstName,
    string LastName,
    string Address,
    string Email,
    string PhoneNumber,
    string PESEL) : ClientRequest(Address, Email, PhoneNumber);

public record CompanyClientRequest(
    string CompanyName,
    string Address,
    string Email,
    string PhoneNumber,
    string KRS) : ClientRequest(Address, Email, PhoneNumber);