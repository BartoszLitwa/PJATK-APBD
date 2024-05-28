using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.API.Data;
using Pharmacy.API.Data.Models;
using Pharmacy.API.Prescriptions.Models.Requests;
using Pharmacy.API.Prescriptions.Models.Responses;

namespace Pharmacy.API.Prescriptions.Commands;

public record AddPrescriptionCommand(AddPrescriptionRequest Request) : IRequest<AddPrescriptionResponse>;

public class AddPrescriptionHandler(PharmacyDbContext context) : IRequestHandler<AddPrescriptionCommand, AddPrescriptionResponse>
{
    public async Task<AddPrescriptionResponse> Handle(AddPrescriptionCommand request, CancellationToken cancellationToken)
    {
        var patientExists = await context.Patients
            .AnyAsync(p => p.IdPatient == request.Request.Patient.IdPatient, cancellationToken);

        if (!patientExists)
        {
            var patient = new Patient
            {
                FirstName = request.Request.Patient.FirstName,
                LastName = request.Request.Patient.LastName,
                Birthdate = request.Request.Patient.Birthdate
            };
            await context.Patients.AddAsync(patient, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        
        if(request.Request.DueDate < request.Request.Date)
            throw new Exception("Due date must be greater than date");
        
        var medicamentsIds = request.Request.Medicaments
            .Select(m => m.IdMedicament)
            .ToList();

        if (medicamentsIds.Count > 10)
            throw new Exception("Medicaments must be unique");

        var medicametsExist = await context.Medicaments
            .Select(m => m.IdMedicament)
            .Where(m => medicamentsIds.Contains(m))
            .ToListAsync(cancellationToken);
        
        if (medicametsExist.Count != medicamentsIds.Count)
            throw new Exception("All Medicaments must exist");

        var prescription = new Prescription
        {
            Date = request.Request.Date,
            DueDate = request.Request.DueDate,
            IdPatient = request.Request.Patient.IdPatient,
            IdDoctor = request.Request.IdDoctor
        };
        await context.Prescriptions.AddAsync(prescription, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var prescriptionMedicaments = request.Request.Medicaments
            .Select(m => new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            })
            .ToList();
        
        await context.PrescriptionMedicaments.AddRangeAsync(prescriptionMedicaments, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddPrescriptionResponse
        {
            IdPrescription = prescription.IdPrescription,
            IdMedicaments = prescriptionMedicaments.Select(m => m.IdMedicament).ToList()
        };
    }
}