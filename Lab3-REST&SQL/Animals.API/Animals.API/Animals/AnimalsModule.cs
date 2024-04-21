using Animals.API.Animals.Commands;
using Animals.API.Animals.Queries;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Animals;

public class AnimalsModule() : CarterModule("/api/animals")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapGet("", async (ISender sender, [FromQuery] string? orderBy = null) =>
        {
            var result = await sender.Send(new GetAllAnimalsQuery(orderBy));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
        
        group.MapGet("{idAnimal:int}", async ([FromRoute] int idAnimal, ISender sender) =>
        {
            var result = await sender.Send(new GetAnimalByIdQuery(idAnimal));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
        
        group.MapPost("", async ([FromBody] CreateAnimalRequestDto request, ISender sender) =>
        {
            var result = await sender.Send(new CreateAnimalCommand(request.Name, request.Description, request.Category, request.Area));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
        
        group.MapPut("{idAnimal:int}", async ([FromRoute] int idAnimal, [FromBody] UpdateAnimalRequestDto request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateAnimalCommand(idAnimal, request));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok();
        });
        
        group.MapDelete("{idAnimal:int}", async ([FromRoute] int idAnimal, ISender sender) =>
        {
            var result = await sender.Send(new DeleteAnimalCommand(idAnimal));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok();
        });
    }
}