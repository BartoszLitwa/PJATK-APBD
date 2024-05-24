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
            var clientIdMax = await _context.Clients.MaxAsync(c => c.IdClient, cancellationToken);
            client = new Client
            {
                IdClient = clientIdMax + 1,
                FirstName = request.dto.FirstName,
                LastName = request.dto.LastName,
                Pesel = request.dto.Pesel,
                Email = request.dto.Email,
                Telephone = request.dto.Telephone,
            };
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
            RegisteredAt = DateTime.UtcNow,
            PaymentDate = DateTime.TryParse(request.dto.PaymentDate, out var paymentDate) ? paymentDate : null
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}