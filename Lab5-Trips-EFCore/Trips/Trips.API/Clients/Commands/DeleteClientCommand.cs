using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Trips.API.Data;

namespace Trips.API.Clients.Commands;

public record DeleteClientCommand(int IdClient) : IRequest<Result>;

public class DeleteClientHandler(TripsDbContext _context) : IRequestHandler<DeleteClientCommand, Result>
{
    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .SingleOrDefaultAsync(c => c.IdClient == request.IdClient, cancellationToken);

        if (client is null)
            return Result.Failure("Client not found");

        if (client.ClientTrips.Count != 0)
            return Result.Failure("Client cannot be deleted because they are registered for one or more trips");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}