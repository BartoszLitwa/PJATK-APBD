using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Trips.API.Data;
using Trips.API.Trips.Models.Responses;

namespace Trips.API.Trips.Queries;

public record GetAllTripsQuery() : IRequest<Result<IEnumerable<GetAllTripsResponse>>>;

public class GetAllTripsHandler(TripsDbContext _context, IMapper _mapper) 
    : IRequestHandler<GetAllTripsQuery, Result<IEnumerable<GetAllTripsResponse>>>
{
    public async Task<Result<IEnumerable<GetAllTripsResponse>>> Handle(GetAllTripsQuery request, CancellationToken cancellationToken)
    {
        var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Select(x => new GetAllTripsResponse
            {
                Name = x.Name,
                Description = x.Description,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                MaxPeople = x.MaxPeople,
                Countries = x.Countries.Select(ct => new CountryResponse { Name = ct.Name }),
                Clients = x.ClientTrips.Select(ct => new ClientResponse
                {
                    FirstName = ct.Client.FirstName,
                    LastName = ct.Client.LastName
                })
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return Result.Success<IEnumerable<GetAllTripsResponse>>(trips);
    }
}