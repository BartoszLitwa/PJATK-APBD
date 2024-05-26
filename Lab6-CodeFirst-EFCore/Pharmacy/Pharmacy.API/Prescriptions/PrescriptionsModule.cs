using Azure.Core;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.API.Prescriptions.Commands;
using Pharmacy.API.Prescriptions.Models.Requests;

namespace Pharmacy.API.Prescriptions;

public class PrescriptionsModule() : CarterModule("/api/prescriptions")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapPost("", async ([FromBody] AddPrescriptionRequest Request, ISender sender) =>
        {
            var response = await sender.Send(new AddPrescriptionCommand(Request));
            return Results.Ok(response);
        });
    }
}