using MediatR;
using RCS.API.Clients.Models.Requests;
using RCS.API.Clients.Repository;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Commands;

public record UpdateClientCommand(UpdateClientRequest Client) : IRequest;

public class UpdateClientHandler(IClientRepository _repository) : IRequestHandler<UpdateClientCommand>
{
    public async Task Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _repository.GetClientAsync(request.Client.Id);
        if (client == null) throw new Exception("Client not found");

        client.FirstName = request.Client.FirstName;
        client.LastName = request.Client.LastName;
        client.Address = request.Client.Address;
        client.Email = request.Client.Email;
        client.PhoneNumber = request.Client.PhoneNumber;

        await _repository.UpdateClientAsync(client);
    }
}