using Carter;
using Kolos.API.Clients.Commands;
using Kolos.API.Clients.Models.Requests;
using Kolos.API.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kolos.API.Clients;

public class SubscriptionsModule() : CarterModule("/api/subscriptions")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapPost("", async ([FromBody] AddPaymentForSubscriptionForClientRequest request, ISender sender) =>
        {
            var response = await sender.Send(
                new AddPaymentForSubscriptionForClientCommand(request.IdClient, request.IdSubscription, request.Payment));
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        });
    }
}