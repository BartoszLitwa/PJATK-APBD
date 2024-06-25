using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Softwares.Commands;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Queries;

namespace RCS.API.Softwares;

public class SoftwaresModule() : CarterModule("/api/softwares")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        group.MapPost("", async (ISender sender, [FromBody] CreateSoftwareRequest createContractRequest) =>
        {
            var result = await sender.Send(new CreateSoftwareCommand(createContractRequest));
            return Results.Ok(result);
        });

        group.MapGet("", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllSoftwareQuery());
            return Results.Ok(result);
        });
    }
}