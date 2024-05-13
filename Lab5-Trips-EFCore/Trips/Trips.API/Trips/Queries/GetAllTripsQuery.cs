using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Trips.API.Data;
using Trips.API.Trips.Models.Responses;

namespace Trips.API.Trips.Queries;

public record GetAllTripsQuery() : IRequest<Result<IEnumerable<GetAllTripsResponse>>>;

public class GetAllTripsHandler(TripsDbContext _context, IMapper _mapper) : IRequestHandler<GetAllTripsQuery, Result<IEnumerable<GetAllTripsResponse>>>
{
    public async Task<Result<IEnumerable<GetAllTripsResponse>>> Handle(GetAllTripsQuery request, CancellationToken cancellationToken)
    {
        var trips = await _context.Trips
            .Include(t => t.Countries)
            .Include(t => t.ClientTrips)
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var mapped = _mapper.Map<IEnumerable<GetAllTripsResponse>>(trips);
        return Result.Success(mapped);
    }
}