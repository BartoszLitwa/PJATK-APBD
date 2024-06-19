using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Models.Responses;
using RCS.API.Contracts.Repository;
using RCS.API.Data;

namespace RCS.API.Contracts.Queries;

public record CalculatePredictedRevenueQuery(CalculateRevenueRequest Request) : IRequest<RevenueResponse>;

public class CalculatePredictedRevenueHandler(IContractRepository _repository)
    : IRequestHandler<CalculatePredictedRevenueQuery, RevenueResponse>
{
    public async Task<RevenueResponse> Handle(CalculatePredictedRevenueQuery request, CancellationToken cancellationToken)
    {
        var predictedRevenue = await _repository.CalculatePredictedRevenueAsync(request.Request.ProductName);
        var exchangeRate = await _repository.GetExchangeRateAsync(request.Request.Currency ?? "PLN");
        var revenueInCurrency = predictedRevenue * exchangeRate;

        return new RevenueResponse(revenueInCurrency, request.Request.Currency ?? "PLN");
    }
}
