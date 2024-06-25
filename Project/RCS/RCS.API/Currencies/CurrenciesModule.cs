using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Softwares.Commands;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Queries;

namespace RCS.API.Softwares;

public class CurrenciesModule() : CarterModule("/api/currencies")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        group.MapPost("", async (ISender sender, [FromBody] CreateCurrencyRequest createCurrencyRequest) =>
        {
            var result = await sender.Send(new CreateCurrencyCommand(createCurrencyRequest));
            return Results.Ok(result);
        });

        group.MapGet("", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllCurrenciesQuery());
            return Results.Ok(result);
        });
    }
}