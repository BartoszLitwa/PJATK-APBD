using Carter;
using Kolos.API.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kolos.API.Clients;

public class ClientsModule() : CarterModule("/api/clients")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapGet("{IdClient:int}", async ([FromRoute] int IdClient, ISender sender) =>
        {
            var response = await sender.Send(new GetClientWithSubscriptionListQuery(IdClient));
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        });
    }
}