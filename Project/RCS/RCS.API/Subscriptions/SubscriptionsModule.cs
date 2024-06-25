using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Softwares.Commands;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Queries;

namespace RCS.API.Softwares;

public class SubscriptionsModule() : CarterModule("/api/subscriptions")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        group.MapPost("", async (ISender sender, [FromBody] CreateSubscriptionRequest createSubscriptionRequest) =>
        {
            var result = await sender.Send(new CreateSubscriptionCommand(createSubscriptionRequest));
            return Results.Ok(result);
        });

        group.MapPost("{subscriptionId:int}/payments", async (ISender sender, int subscriptionId, [FromBody] IssueSubscriptionPaymentRequest issueSubscriptionPaymentRequest) =>
        {
            var result = await sender.Send(new IssueSubscriptionPaymentCommand(issueSubscriptionPaymentRequest));
            return Results.Ok(result);
        });
    }
}