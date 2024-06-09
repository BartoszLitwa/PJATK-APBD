using CSharpFunctionalExtensions;
using Kolos.API.Clients.Models.Responses;
using Kolos.API.Data;
using Kolos.API.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kolos.API.Clients.Commands;

public record AddPaymentForSubscriptionForClientCommand(int IdClient, int IdSubscription, decimal Payment)
    : IRequest<Result<AddPaymentForSubscriptionForClientResponse, string>>;
    
public class AddPaymentForSubscriptionForClientHandler(KolosDbContext context) 
    : IRequestHandler<AddPaymentForSubscriptionForClientCommand, Result<AddPaymentForSubscriptionForClientResponse, string>>
{
    public async Task<Result<AddPaymentForSubscriptionForClientResponse, string>> Handle(AddPaymentForSubscriptionForClientCommand request, CancellationToken cancellationToken)
    {
        var client = await context.Clients
            .Include(c => c.Discounts)
            .FirstOrDefaultAsync(p => p.IdClient == request.IdClient, cancellationToken);
        if (client is null)
            return Result.Failure<AddPaymentForSubscriptionForClientResponse, string>("Client not found");
        
        var sale = await context.Sales
            .Include(s => s.Subscription)
            .ThenInclude(s => s.Payments)
            .FirstOrDefaultAsync(p => p.IdClient == request.IdClient && p.IdSubscription == request.IdSubscription, cancellationToken);
        if (sale is null)
            return Result.Failure<AddPaymentForSubscriptionForClientResponse, string>("Subscription not found");
        
        if(sale.CreatedAt.Date >= DateTime.UtcNow.Date && sale.Subscription.EndTime.Date < DateTime.UtcNow.Date)
            return Result.Failure<AddPaymentForSubscriptionForClientResponse, string>("Subscription has to be active");
        
        var hasAlreadyPaid = sale.Subscription.Payments
            .Any(p => p.Date.Date <= sale.Subscription.EndTime.Date 
                      && p.Date.Date >= sale.CreatedAt.Date);
        if (hasAlreadyPaid)
            return Result.Failure<AddPaymentForSubscriptionForClientResponse, string>("Subscription for this period has been paid");

        if(sale.Subscription.Price != request.Payment)
            return Result.Failure<AddPaymentForSubscriptionForClientResponse, string>("Payment must be equal to subscription price");
        
        decimal discounts = client.Discounts.Sum(d => d.Value);
        discounts = discounts > 50 ? 50 : discounts;
        
        decimal newPaymentPrice = request.Payment * (1 - (discounts / 100));

        var payment = new Payment
        {
            Date = DateTime.UtcNow.Date,
            IdClient = request.IdClient,
            IdSubscription = request.IdSubscription,
        };
        await context.Payments.AddAsync(payment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddPaymentForSubscriptionForClientResponse()
        {
            IdPayment = payment.IdPayment,
        };
    }
}