using MediatR;
using RCS.API.Clients.Models.Requests;
using RCS.API.Clients.Repository;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Commands;

public record AddClientCommand(AddClientRequest Client) : IRequest<int>;

public class AddClientHandler(IClientRepository _repository) : IRequestHandler<AddClientCommand, int>
{
    public async Task<int> Handle(AddClientCommand request, CancellationToken cancellationToken)
    {
        var client = new IndividualClient
        {
            FirstName = request.Client.FirstName,
            LastName = request.Client.LastName,
            Address = request.Client.Address,
            Email = request.Client.Email,
            PhoneNumber = request.Client.PhoneNumber,
            PESEL = request.Client.PESEL
        };
        
        return await _repository.AddClientAsync(client);
    }
}