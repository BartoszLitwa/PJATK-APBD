using Animals.API.Animals.Commands;
using Animals.API.Animals.Queries;
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
            var result = await sender.Send(new GetAllAnimalsQuery(orderBy));
        });
        
        group.MapPost("", async ([FromBody] CreateAnimalRequestDto request, ISender sender) =>
        {
            var result = await sender.Send(new CreateAnimalCommand(request.Name, request.Description, request.Category, request.Area));
        });
        
        group.MapPut("{id:int}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new GetAnimalByIdQuery(id));
        });
        
        group.MapDelete("{id:int}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteAnimalCommand(id));
        });
    }
}