using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trips.API.Trips.Commands;
using Trips.API.Trips.Models.Requests;
using Trips.API.Trips.Queries;

namespace Trips.API.Trips;

public class TripsModule() : CarterModule("api/trips")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");
        
        group.MapGet("", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllTripsQuery());
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
        
        group.MapPost("{idTrip:int}/clients", async (IMediator mediator, [FromRoute] int idTrip, AssignClientToTripRequest clientDto) =>
        {
            var result = await mediator.Send(new AssignClientToTripCommand(idTrip, clientDto));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });
    }
}