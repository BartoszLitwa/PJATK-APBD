using MediatR;
using RCS.API.Data.Models;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Models.Responses;
using RCS.API.Softwares.Repository;

namespace RCS.API.Softwares.Commands;

public record IssueSubscriptionPaymentCommand(IssueSubscriptionPaymentRequest Request) : IRequest<IssueSubscriptionPaymentResponse>;

public class IssueSubscriptionPaymentHandler(ISubscriptionRepository subscriptionRepository)
    : IRequestHandler<IssueSubscriptionPaymentCommand, IssueSubscriptionPaymentResponse>
{
    private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;

    public async Task<IssueSubscriptionPaymentResponse> Handle(IssueSubscriptionPaymentCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionAsync(request.Request.SubscriptionId);
        if (subscription == null) return new IssueSubscriptionPaymentResponse(false, "Subscription not found");

        if (!subscription.IsActive)
        {
            return new IssueSubscriptionPaymentResponse(false, "Subscription is not active");
        }

        if (DateTime.Now > subscription.NextRenewalDate)
        {
            subscription.IsActive = false;
            await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
            return new IssueSubscriptionPaymentResponse(false, "Payment deadline has passed, subscription cancelled");
        }

        var totalPaid = subscription.Payments.Sum(p => p.Amount);
        if (totalPaid >= subscription.Price)
        {
            return new IssueSubscriptionPaymentResponse(false, "Payment already completed for the current period");
        }

        if (request.Request.Amount != subscription.Price)
        {
            return new IssueSubscriptionPaymentResponse(false, "Payment amount must equal the renewal period amount");
        }

        var payment = new SubscriptionPayment
        {
            SubscriptionId = request.Request.SubscriptionId,
            Amount = request.Request.Amount,
            PaymentDate = DateTime.Now
        };

        await _subscriptionRepository.AddSubscriptionPaymentAsync(payment);

        subscription.NextRenewalDate = subscription.NextRenewalDate.AddMonths(subscription.RenewalPeriod == "monthly" ? 1 : 12);
        await _subscriptionRepository.UpdateSubscriptionAsync(subscription);

        return new IssueSubscriptionPaymentResponse(true, "Payment successful");
    }
}
