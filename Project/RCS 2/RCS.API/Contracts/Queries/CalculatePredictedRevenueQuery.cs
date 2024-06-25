using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Models.Responses;
using RCS.API.Contracts.Repository;
using RCS.API.Data;
using RCS.API.Softwares.Repository;

namespace RCS.API.Contracts.Queries;

public record CalculatePredictedRevenueQuery(CalculateRevenueRequest Request) : IRequest<RevenueResponse>;

public class CalculatePredictedRevenueHandler(IContractRepository _repository, ICurrencyRepository _currencyRepository)
    : IRequestHandler<CalculatePredictedRevenueQuery, RevenueResponse>
{
    public async Task<RevenueResponse> Handle(CalculatePredictedRevenueQuery request, CancellationToken cancellationToken)
    {
        var predictedRevenue = await _repository.CalculatePredictedRevenueAsync(request.Request.ProductName);
        var currency = await _currencyRepository.GetCurrencyAsync(request.Request.Currency ?? "PLN");
        if (currency == null)
        {
            throw new Exception("Currency not found");
        }
        
        var revenueInCurrency = predictedRevenue * currency.ExchangeRateToPLN;

        return new RevenueResponse(revenueInCurrency, request.Request.Currency ?? "PLN");
    }
}
