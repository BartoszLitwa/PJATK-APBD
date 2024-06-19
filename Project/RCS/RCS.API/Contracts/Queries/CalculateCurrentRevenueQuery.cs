using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Data;

namespace RCS.API.Contracts.Queries;

public record CalculateCurrentRevenueQuery : IRequest<decimal>;

public class CalculateCurrentRevenueHandler(RcsDbContext context) : IRequestHandler<CalculateCurrentRevenueQuery, decimal>
{
    public async Task<decimal> Handle(CalculateCurrentRevenueQuery request, CancellationToken cancellationToken)
    {
        var revenue = await context.Contracts.Where(c => c.IsSigned)
            .SumAsync(c => c.Price, cancellationToken);
        return revenue;
    }
}