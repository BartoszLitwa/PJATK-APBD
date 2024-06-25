using MediatR;
using RCS.API.Clients.Repository;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Models.Responses;
using RCS.API.Contracts.Repository;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Contracts.Commands;

public record CreateContractCommand(CreateContractRequest Request) : IRequest<CreateContractResponse>;

public class CreateContractHandler(IClientRepository _clientRepository, IContractRepository _contractRepository) 
    : IRequestHandler<CreateContractCommand, CreateContractResponse>
{
    public async Task<CreateContractResponse> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientAsync(request.Request.ClientId);
        if (client == null) throw new Exception("Client not found");

        var software = await _contractRepository.GetSoftwareAsync(request.Request.SoftwareId);
        if (software == null) throw new Exception("Software not found");

        if (await _contractRepository.HasActiveContractAsync(request.Request.ClientId, request.Request.SoftwareId))
            throw new Exception("Client already has an active contract for this software");
        
        var contractDuration = (request.Request.EndDate - request.Request.StartDate).Days;
        if (contractDuration < 3 || contractDuration > 30)
            throw new Exception("Contract duration must be between 3 and 30 days");

        var hasPreviousContract = await _contractRepository.HasPreviousContractAsync(request.Request.ClientId);
        var discount = hasPreviousContract ? 0.05m : 0m;

        var price = software.Price * (1 - discount) + request.Request.AdditionalYearsOfSupport * 1000m;

        var contract = new Contract
        {
            ClientId = request.Request.ClientId,
            SoftwareId = request.Request.SoftwareId,
            StartDate = request.Request.StartDate,
            EndDate = request.Request.EndDate,
            Price = price
        };

        var contractId = await _contractRepository.CreateContractAsync(contract);

        return new CreateContractResponse(contractId, contract.Price);
    }
}