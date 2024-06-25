using MediatR;
using RCS.API.Clients.Models.Responses;
using RCS.API.Clients.Repository;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Commands;

public record DeleteClientCommand(int IdClient) : IRequest<ClientResponse>;

public class DeleteClientHandler(IClientRepository _repository) : IRequestHandler<DeleteClientCommand, ClientResponse>
{
    public async Task<ClientResponse> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _repository.GetClientAsync(request.IdClient);
        if (client == null) throw new Exception("Client not found");

        await _repository.DeleteClientAsync(request.IdClient);
        return client switch
        {
            IndividualClient individualClient => IndividualClientResponse.From(individualClient),
            CompanyClient companyClient => CompanyClientResponse.From(companyClient),
            _ => throw new Exception("Unknown client type")
        };
    }
}