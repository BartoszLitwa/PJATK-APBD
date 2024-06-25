using MediatR;
using RCS.API.Clients.Repository;
using RCS.API.Contracts.Repository;
using RCS.API.Data.Models;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Models.Responses;
using RCS.API.Softwares.Repository;

namespace RCS.API.Softwares.Commands;

public record CreateSubscriptionCommand(CreateSubscriptionRequest Request) : IRequest<CreateSubscriptionResponse>;

public class CreateSubscriptionHandler(
    IClientRepository clientRepository,
    IContractRepository contractRepository,
    ISoftwareRepository softwareRepository,
    ISubscriptionRepository subscriptionRepository)
    : IRequestHandler<CreateSubscriptionCommand, CreateSubscriptionResponse>
{
    public async Task<CreateSubscriptionResponse> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetClientAsync(request.Request.ClientId);
        if (client == null) throw new Exception("Client not found");

        var software = await softwareRepository.GetSoftwareAsync(request.Request.SoftwareId);
        if (software == null) throw new Exception("Software not found");

        var hasPreviousContract = await contractRepository.HasPreviousContractAsync(request.Request.ClientId);
        var discount = hasPreviousContract ? 0.05m : 0m;

        var price = request.Request.Price * (1 - discount);

        var subscription = new Subscription
        {
            ClientId = request.Request.ClientId,
            SoftwareId = request.Request.SoftwareId,
            Name = request.Request.Name,
            RenewalPeriod = request.Request.RenewalPeriod,
            Price = price,
            StartDate = DateTime.Now,
            NextRenewalDate = DateTime.Now.AddMonths(request.Request.RenewalPeriod == "monthly" ? 1 : 12),
            IsActive = true
        };

        var subscriptionCreated = await subscriptionRepository.CreateSubscriptionAsync(subscription);

        return new CreateSubscriptionResponse(subscriptionCreated.Id, price);
    }
}
