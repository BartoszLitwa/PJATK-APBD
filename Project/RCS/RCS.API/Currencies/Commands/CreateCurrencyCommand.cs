using MediatR;
using RCS.API.Data.Models;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Models.Responses;
using RCS.API.Softwares.Repository;

namespace RCS.API.Softwares.Commands;

public record CreateCurrencyCommand(CreateCurrencyRequest Request) : IRequest<CreateCurrencyResponse>;

public class CreateCurrencyHandler(ICurrencyRepository _currencyRepository)
    : IRequestHandler<CreateCurrencyCommand, CreateCurrencyResponse>
{
    public async Task<CreateCurrencyResponse> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = new Currency
        {
            Code = request.Request.Code,
            ExchangeRateToPLN = request.Request.ExchangeRateToPLN
        };

        var currencyCreated = await _currencyRepository.AddCurrencyAsync(currency);

        return new CreateCurrencyResponse(currencyCreated.Id, currency.Code);
    }
}
