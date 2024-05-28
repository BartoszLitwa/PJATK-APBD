using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.API.Data;
using Pharmacy.API.Patients.Models.Responses;

namespace Pharmacy.API.Patients.Queries;

public record GetAllPatientInfoWithPrescriptionsAndMedicamentsQuery(int IdPatient) : IRequest<PatientInfoWithPrescriptionsAndMedicamentsResponse>;

public class GetAllPatientInfoWithPrescriptionsAndMedicamentsHandler(PharmacyDbContext context)
    : IRequestHandler<GetAllPatientInfoWithPrescriptionsAndMedicamentsQuery, PatientInfoWithPrescriptionsAndMedicamentsResponse>
{
    public async Task<PatientInfoWithPrescriptionsAndMedicamentsResponse> Handle(GetAllPatientInfoWithPrescriptionsAndMedicamentsQuery request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients
            .Select(x => new PatientInfoWithPrescriptionsAndMedicamentsResponse
            {
                IdPatient = x.IdPatient,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthdate = x.Birthdate,
                Prescriptions = x.Prescriptions
                    .Select(p => new PrescriptionInfoWithMedicamentsResponse
                    {
                        IdPrescription = p.IdPrescription,
                        Date = p.Date,
                        DueDate = p.DueDate,
                        Medicaments = p.PrescriptionMedicaments
                            .Select(pm => new MedicamentInfoResponse
                            {
                                IdMedicament = pm.IdMedicament,
                                Name = pm.Medicament.Name,
                                Description = pm.Medicament.Description,
                                Type = pm.Medicament.Type,
                                Dose = pm.Dose,
                            }).ToList(),
                        Doctor = new DoctorInfoResponse
                        {
                            IdDoctor = p.Doctor.IdDoctor,
                            FirstName = p.Doctor.FirstName,
                            LastName = p.Doctor.LastName,
                            Email = p.Doctor.Email,
                        }
                    })
                    .OrderBy(p => p.DueDate)
                    .ToList()
            })
            .FirstOrDefaultAsync(x => x.IdPatient == request.IdPatient, cancellationToken);
        
        if(patient is null)
            throw new Exception("Patient not found");
        
        return patient;
    }
}