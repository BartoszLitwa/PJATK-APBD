using RCS.API.Data.Models;

namespace RCS.API.Clients.Models.Responses;

public abstract record ClientResponse(
    int Id,
    string Address,
    string Email,
    string PhoneNumber);

public record IndividualClientResponse(
    int Id,
    string FirstName,
    string LastName,
    string Address,
    string Email,
    string PhoneNumber,
    string PESEL,
    bool IsDeleted) : ClientResponse(Id, Address, Email, PhoneNumber)
{
    public static IndividualClientResponse From(IndividualClient client) => new(
        client.Id,
        client.FirstName,
        client.LastName,
        client.Address,
        client.Email,
        client.PhoneNumber,
        client.PESEL,
        client.IsDeleted);
}

public record CompanyClientResponse(
    int Id,
    string CompanyName,
    string Address,
    string Email,
    string PhoneNumber,
    string KRS) : ClientResponse(Id, Address, Email, PhoneNumber)
{
    public static CompanyClientResponse From(CompanyClient client) => new(
        client.Id,
        client.CompanyName,
        client.Address,
        client.Email,
        client.PhoneNumber,
        client.KRS);
}