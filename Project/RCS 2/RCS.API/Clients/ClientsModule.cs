using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Clients.Commands;
using RCS.API.Clients.Models.Requests;
using RCS.API.Clients.Queries;
using RCS.API.Common;
using RCS.API.Contracts.Commands;

namespace RCS.API.Clients;

public class ClientsModule() : CarterModule("/api/clients")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        group.MapGet("{idClient:int}", async ([FromRoute] int idClient, ISender sender) =>
        {
            try
            {
                var response = await sender.Send(new GetClientByIdQuery(idClient));
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        group.MapPost("/individual", async ([FromBody] IndividualClientRequest request, ISender sender) =>
        {
            var response = await sender.Send(new AddClientCommand(request));
            return Results.Ok(response);
        });
        
        group.MapPost("/company", async ([FromBody] CompanyClientRequest request, ISender sender) =>
        {
            var response = await sender.Send(new AddClientCommand(request));
            return Results.Ok(response);
        });
        
        group.MapPut("/individual/{idClient:int}", async ([FromRoute] int idClient, [FromBody] IndividualClientRequest request, ISender sender, IJwtService jwt) =>
        {
            if (!jwt.IsAdmin)
                return Results.Forbid();
            
            var response = await sender.Send(new UpdateClientCommand(idClient, request));
            return Results.Ok(response);
        });
        
        group.MapPut("/company/{idClient:int}", async ([FromRoute] int idClient, [FromBody] CompanyClientRequest request, ISender sender, IJwtService jwt) =>
        {
            if (!jwt.IsAdmin)
                return Results.Forbid();
            
            var response = await sender.Send(new UpdateClientCommand(idClient, request));
            return Results.Ok(response);
        });
        
        group.MapDelete("/individual/{idClient:int}", async ([FromRoute] int idClient, ISender sender, IJwtService jwt) =>
        {
            if (!jwt.IsAdmin)
                return Results.Forbid();
            
            var response = await sender.Send(new DeleteClientCommand(idClient));
            return Results.Ok(response);
        });
        
        group.MapDelete("/company/{idClient:int}", async ([FromRoute] int idClient, ISender sender, IJwtService jwt) =>
        {
            if (!jwt.IsAdmin)
                return Results.Forbid();
            
            var response = await sender.Send(new DeleteClientCommand(idClient));
            return Results.Ok(response);
        });
    }
}