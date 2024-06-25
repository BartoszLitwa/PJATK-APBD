using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Softwares.Repository;

public class SubscriptionRepository(RcsDbContext _context) : ISubscriptionRepository
{
    public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription?> GetSubscriptionAsync(int subscriptionId)
    {
        return await _context.Subscriptions.Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task AddSubscriptionPaymentAsync(SubscriptionPayment payment)
    {
        _context.SubscriptionPayments.Add(payment);
        await _context.SaveChangesAsync();
    }
}
