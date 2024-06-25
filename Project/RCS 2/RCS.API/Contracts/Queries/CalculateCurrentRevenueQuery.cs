using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Models.Responses;
using RCS.API.Contracts.Repository;
using RCS.API.Data;
using RCS.API.Softwares.Repository;

namespace RCS.API.Contracts.Queries;

public record CalculateCurrentRevenueQuery(CalculateRevenueRequest Request) : IRequest<RevenueResponse>;

public class CalculateCurrentRevenueHandler(IContractRepository _repository, ICurrencyRepository _currencyRepository) 
    : IRequestHandler<CalculateCurrentRevenueQuery, RevenueResponse>
{
    public async Task<RevenueResponse> Handle(CalculateCurrentRevenueQuery request, CancellationToken cancellationToken)
    {
        var currentRevenue = await _repository.CalculateCurrentRevenueAsync(request.Request.ProductName);
        var currency = await _currencyRepository.GetCurrencyAsync(request.Request.Currency ?? "PLN");
        if (currency == null)
        {
            throw new Exception("Currency not found");
        }
        
        var revenueInCurrency = currentRevenue * currency.ExchangeRateToPLN;

        return new RevenueResponse(revenueInCurrency, request.Request.Currency ?? "PLN");
    }
}
