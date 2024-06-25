using MediatR;
using RCS.API.Clients.Models.Requests;
using RCS.API.Clients.Models.Responses;
using RCS.API.Clients.Repository;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Commands;

public record UpdateClientCommand(int IdClient, ClientRequest Client) : IRequest<ClientResponse>;

public class UpdateClientHandler(IClientRepository _repository) : IRequestHandler<UpdateClientCommand, ClientResponse>
{
    public async Task<ClientResponse> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var existingClient = await _repository.GetClientAsync(request.IdClient);
        if (existingClient == null) throw new Exception("Client not found");

        switch (request.Client)
        {
            case IndividualClientRequest individualClientRequest:
                if (existingClient is not IndividualClient existingIndividualClient)
                    throw new Exception("Client type mismatch");

                // Ensure PESEL cannot be changed
                if (existingIndividualClient.PESEL != individualClientRequest.PESEL)
                    throw new Exception("PESEL cannot be changed");

                existingIndividualClient.FirstName = individualClientRequest.FirstName;
                existingIndividualClient.LastName = individualClientRequest.LastName;
                existingIndividualClient.Address = individualClientRequest.Address;
                existingIndividualClient.Email = individualClientRequest.Email;
                existingIndividualClient.PhoneNumber = individualClientRequest.PhoneNumber;

                await _repository.UpdateClientAsync(existingIndividualClient);
                return IndividualClientResponse.From(existingIndividualClient);

            case CompanyClientRequest companyClientRequest:
                if (existingClient is not CompanyClient existingCompanyClient)
                    throw new Exception("Client type mismatch");

                // Ensure KRS cannot be changed
                if (existingCompanyClient.KRS != companyClientRequest.KRS)
                    throw new Exception("KRS cannot be changed");

                existingCompanyClient.CompanyName = companyClientRequest.CompanyName;
                existingCompanyClient.Address = companyClientRequest.Address;
                existingCompanyClient.Email = companyClientRequest.Email;
                existingCompanyClient.PhoneNumber = companyClientRequest.PhoneNumber;

                await _repository.UpdateClientAsync(existingCompanyClient);
                return CompanyClientResponse.From(existingCompanyClient);

            default:
                throw new Exception("Unknown client type");
        }
    }
}