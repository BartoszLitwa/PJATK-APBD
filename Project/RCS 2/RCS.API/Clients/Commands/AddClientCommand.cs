using MediatR;
using RCS.API.Clients.Models.Requests;
using RCS.API.Clients.Models.Responses;
using RCS.API.Clients.Repository;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Commands;

public record AddClientCommand(ClientRequest Client) : IRequest<ClientResponse>;

public class AddClientHandler(IClientRepository _repository) : IRequestHandler<AddClientCommand, ClientResponse>
{
    public async Task<ClientResponse> Handle(AddClientCommand request, CancellationToken cancellationToken)
    {
        Client client = request.Client switch
        {
            IndividualClientRequest individualClientRequest => new IndividualClient
            {
                FirstName = individualClientRequest.FirstName,
                LastName = individualClientRequest.LastName,
                Address = individualClientRequest.Address,
                Email = individualClientRequest.Email,
                PhoneNumber = individualClientRequest.PhoneNumber,
                PESEL = individualClientRequest.PESEL
            },
            CompanyClientRequest companyClientRequest => new CompanyClient
            {
                CompanyName = companyClientRequest.CompanyName,
                Address = companyClientRequest.Address,
                Email = companyClientRequest.Email,
                PhoneNumber = companyClientRequest.PhoneNumber,
                KRS = companyClientRequest.KRS
            },
            _ => throw new Exception("Unknown client type")
        };

        var created = await _repository.AddClientAsync(client);
        return created is IndividualClient individualClient 
            ? IndividualClientResponse.From(individualClient) 
            : CompanyClientResponse.From((CompanyClient)created);
    }
}