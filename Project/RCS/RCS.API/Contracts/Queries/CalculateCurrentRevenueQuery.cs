using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Models.Responses;
using RCS.API.Contracts.Repository;
using RCS.API.Data;

namespace RCS.API.Contracts.Queries;

public record CalculateCurrentRevenueQuery(CalculateRevenueRequest Request) : IRequest<RevenueResponse>;

public class CalculateCurrentRevenueHandler(IContractRepository _repository) 
    : IRequestHandler<CalculateCurrentRevenueQuery, RevenueResponse>
{
    public async Task<RevenueResponse> Handle(CalculateCurrentRevenueQuery request, CancellationToken cancellationToken)
    {
        var currentRevenue = await _repository.CalculateCurrentRevenueAsync(request.Request.ProductName);
        var exchangeRate = await _repository.GetExchangeRateAsync(request.Request.Currency ?? "PLN");
        var revenueInCurrency = currentRevenue * exchangeRate;

        return new RevenueResponse(revenueInCurrency, request.Request.Currency ?? "PLN");
    }
}
