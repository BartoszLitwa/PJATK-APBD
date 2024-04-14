using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Animals;

public class AnimalsModule : CarterModule
{
    public AnimalsModule() : base("/api/animals"){}
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapGet("", async ([FromQuery] string orderBy, ISender sender) =>
        {
            var result = await sender.Send(new());
        });
    }
}