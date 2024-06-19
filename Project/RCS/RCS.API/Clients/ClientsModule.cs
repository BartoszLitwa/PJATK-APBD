using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Clients.Queries;

namespace RCS.API.Clients;

public class ClientsModule() : CarterModule("/api/clients")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapGet("{IdClient:int}", async ([FromRoute] int IdClient, ISender sender) =>
        {
            try
            {
                var response = await sender.Send(new GetClientByIdQuery(IdClient));
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
    }
}