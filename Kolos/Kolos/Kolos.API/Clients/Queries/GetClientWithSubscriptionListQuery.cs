using CSharpFunctionalExtensions;
using Kolos.API.Clients.Models.Responses;
using Kolos.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kolos.API.Clients.Queries;

public record GetClientWithSubscriptionListQuery(int IdClient) 
    : IRequest<Result<GetClientWithSubscriptionListResponse, string>>;

public class GetClientWithSubscriptionListHandler(KolosDbContext context)
    : IRequestHandler<GetClientWithSubscriptionListQuery, Result<GetClientWithSubscriptionListResponse, string>>
{
    public async Task<Result<GetClientWithSubscriptionListResponse, string>> Handle(GetClientWithSubscriptionListQuery request, CancellationToken cancellationToken)
    {
        var client = await context.Clients
            .Select(c => new {
                IdClient = c.IdClient,
                Response = new GetClientWithSubscriptionListResponse
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Discount = c.Discounts
                        .FirstOrDefault(d => DateTime.UtcNow.Date >= d.DateFrom.Date && DateTime.UtcNow.Date <= d.DateTo.Date)
                        .Value,
                    Subscriptions = c.Sales
                        .Select(s => s.Subscription)
                        .Select(s => new GetSubscriptionsResponse
                        {
                            IdSubscription = s.IdSubscription,
                            Name = s.Name,
                            RenewallPeriod = s.RenewallPeriod,
                            TotalPaidAmount = (int)(c.Payments
                                .Count(p => p.IdSubscription == s.IdSubscription) * s.Price)
                        })
                }})
            .FirstOrDefaultAsync(c => c.IdClient == request.IdClient, cancellationToken);
        
        if(client is null)
            return Result.Failure<GetClientWithSubscriptionListResponse, string>("Client not found");
        
        return client.Response;
    }
}