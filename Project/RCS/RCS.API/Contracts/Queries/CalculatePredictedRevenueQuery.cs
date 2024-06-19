using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Data;

namespace RCS.API.Contracts.Queries;

public record CalculatePredictedRevenueQuery : IRequest<decimal>;

public class CalculatePredictedRevenueHandler(RcsDbContext context) : IRequestHandler<CalculatePredictedRevenueQuery, decimal>
{
    public async Task<decimal> Handle(CalculatePredictedRevenueQuery request, CancellationToken cancellationToken)
    {
        var currentRevenue = await context.Contracts.Where(c => c.IsSigned)
            .SumAsync(c => c.Price, cancellationToken);
        var pendingContracts = await context.Contracts.Where(c => !c.IsSigned)
            .SumAsync(c => c.Price, cancellationToken);
        
        return currentRevenue + pendingContracts;
    }
}