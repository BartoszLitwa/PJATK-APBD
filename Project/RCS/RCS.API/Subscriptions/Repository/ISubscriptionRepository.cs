using RCS.API.Data.Models;

namespace RCS.API.Softwares.Repository;

public interface ISubscriptionRepository
{
    Task<Subscription> CreateSubscriptionAsync(Subscription subscription);
    Task<Subscription?> GetSubscriptionAsync(int subscriptionId);
    Task UpdateSubscriptionAsync(Subscription subscription);
    Task AddSubscriptionPaymentAsync(SubscriptionPayment payment);
}