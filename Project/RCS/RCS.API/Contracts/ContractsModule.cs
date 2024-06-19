using Carter;
using MediatR;
using RCS.API.Contracts.Commands;

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
        
        group.MapPost("/{contractId:int}/payments", async (ISender sender, int contractId) =>
        {
            var result = await sender.Send(new IssuePaymentCommand(contractId));
            return result == null ? Results.NotFound() : Results.Ok(result);
        });
    }
}