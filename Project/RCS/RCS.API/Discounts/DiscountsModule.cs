using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Discounts.Commands;
using RCS.API.Discounts.Models.Requests;
using RCS.API.Discounts.Queries;
using RCS.API.Softwares.Commands;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Queries;

namespace RCS.API.Discounts;

public class DiscountsModule() : CarterModule("/api/discounts")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        group.MapPost("", async (ISender sender, [FromBody] CreateDiscountRequest createDiscountRequest) =>
        {
            var result = await sender.Send(new CreateDiscountCommand(createDiscountRequest));
            return Results.Ok(result);
        });

        group.MapGet("", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllDiscountsQuery());
            return Results.Ok(result);
        });
    }
}