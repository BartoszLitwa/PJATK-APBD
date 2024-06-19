using MediatR;
using RCS.API.Clients.Repository;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Commands;

public record SoftDeleteClientCommand(int ClientId) : IRequest;

public class SoftDeleteClientHandler(IClientRepository _repository) : IRequestHandler<SoftDeleteClientCommand>
{
    public async Task Handle(SoftDeleteClientCommand request, CancellationToken cancellationToken)
    {
        await _repository.SoftDeleteClientAsync(request.ClientId);
    }
}