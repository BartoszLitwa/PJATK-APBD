using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trips.API.Clients.Commands;

namespace Trips.API.Clients;

public class ClientsModule() : CarterModule("api/clients")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");
        
        group.MapDelete("{idClient:int}", async (IMediator mediator, [FromRoute]int idClient) =>
        {
            var result = await mediator.Send(new DeleteClientCommand(idClient));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });
    }
}