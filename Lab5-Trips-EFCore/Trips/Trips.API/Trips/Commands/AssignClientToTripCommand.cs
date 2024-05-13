using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Trips.API.Data;
using Trips.API.Data.Models;
using Trips.API.Trips.Models.Requests;

namespace Trips.API.Trips.Commands;

public record AssignClientToTripCommand(int IdTrip, AssignClientToTripRequest dto) : IRequest<Result>;

public class AssignClientToTripHandler(TripsDbContext _context, IMapper _mapper) : IRequestHandler<AssignClientToTripCommand, Result>
{
    public async Task<Result> Handle(AssignClientToTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips.FindAsync([request.IdTrip], cancellationToken);
        if (trip is null)
            return Result.Failure("Trip not found");

        var client = await _context.Clients.SingleOrDefaultAsync(c => c.Pesel == request.dto.Pesel, cancellationToken);
        if (client is null)
        {
            client = _mapper.Map<Client>(request.dto);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var clientTripExists = await _context.ClientTrips
            .AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == request.IdTrip, cancellationToken);
        if (clientTripExists)
            return Result.Failure("Client is already registered for this trip");

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = request.IdTrip,
            RegisteredAt = DateTime.Now
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}