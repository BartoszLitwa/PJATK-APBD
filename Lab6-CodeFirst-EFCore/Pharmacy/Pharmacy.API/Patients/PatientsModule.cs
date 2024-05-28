using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.API.Patients.Queries;

namespace Pharmacy.API.Patients;

public class PatientsModule() : CarterModule("/api/patients")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("");

        group.MapGet("{IdPatient:int}", async ([FromRoute] int IdPatient, ISender sender) =>
        {
            try
            {
                var response = await sender.Send(new GetAllPatientInfoWithPrescriptionsAndMedicamentsQuery(IdPatient));
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
    }
}