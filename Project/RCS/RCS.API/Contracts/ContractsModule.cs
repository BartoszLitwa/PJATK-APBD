using Carter;
using MediatR;
using RCS.API.Contracts.Commands;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Queries;

namespace RCS.API.Contracts;

public class ContractsModule() : CarterModule("/api/contracts")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapPost("", async (ISender sender, CreateContractRequest createContractRequest) =>
        {
            var result = await sender.Send(new CreateContractCommand(createContractRequest));
            return Results.Ok(result);
        });
        
        group.MapPost("/{contractId:int}/payments", async (ISender sender, int contractId, IssuePaymentRequest request) =>
        {
            var result = await sender.Send(new IssuePaymentCommand(request));
            return Results.Ok(result);
        });
        
        group.MapGet("/{contractId:int}/revenue/current", async (ISender sender, int contractId, CalculateRevenueRequest request) =>
        {
            var result = await sender.Send(new CalculateCurrentRevenueQuery(request));
            return Results.Ok(result);
        });
        
        group.MapGet("/{contractId:int}/revenue/predicted", async (ISender sender, int contractId, CalculateRevenueRequest request) =>
        {
            var result = await sender.Send(new CalculatePredictedRevenueQuery(request));
            return Results.Ok(result);
        });
    }
}