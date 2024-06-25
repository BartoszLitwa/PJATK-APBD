using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.API.Auth.Commands;
using RCS.API.Auth.Models.Requests;

namespace RCS.API.Auth;

public class AuthModule() : CarterModule("/api/auth")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapPost("login", async ([FromBody] LoginRequest request, ISender sender) =>
        {
            try
            {
                var response = await sender.Send(new LoginCommand(request));
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        
        group.MapPost("register", async ([FromBody] CreateEmployeeRequest request, ISender sender) =>
        {
            try
            {
                var response = await sender.Send(new RegisterEmployeeCommand(request));
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
    }
}