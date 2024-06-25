using MediatR;
using RCS.API.Softwares.Models.Responses;
using RCS.API.Softwares.Repository;

namespace RCS.API.Softwares.Queries;

public record GetAllCurrenciesQuery : IRequest<IEnumerable<CreateCurrencyResponse>>;

public class GetAllCurrenciesHandler : IRequestHandler<GetAllCurrenciesQuery, IEnumerable<CreateCurrencyResponse>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetAllCurrenciesHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<IEnumerable<CreateCurrencyResponse>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await _currencyRepository.GetAllCurrenciesAsync();

        return currencies.Select(c => new CreateCurrencyResponse(c.Id, c.Code));
    }
}
