using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Contracts.Commands;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Queries;

namespace RCS.API.Contracts;

public class ContractsModule() : CarterModule("/api/contracts")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        group.MapPost("", async (ISender sender, [FromBody] CreateContractRequest createContractRequest) =>
        {
            var result = await sender.Send(new CreateContractCommand(createContractRequest));
            return Results.Ok(result);
        });
        
        group.MapPost("/{contractId:int}/payments", async (ISender sender, int contractId, [FromBody] IssuePaymentRequest request) =>
        {
            var result = await sender.Send(new IssuePaymentCommand(request));
            return Results.Ok(result);
        });
        
        group.MapPost("/revenue/current", async (ISender sender, [FromBody] CalculateRevenueRequest request) =>
        {
            var result = await sender.Send(new CalculateCurrentRevenueQuery(request));
            return Results.Ok(result);
        });
        
        group.MapPost("/revenue/predicted", async (ISender sender, [FromBody] CalculateRevenueRequest request) =>
        {
            var result = await sender.Send(new CalculatePredictedRevenueQuery(request));
            return Results.Ok(result);
        });
    }
}