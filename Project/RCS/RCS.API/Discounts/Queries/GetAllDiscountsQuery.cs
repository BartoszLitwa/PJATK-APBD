using MediatR;
using RCS.API.Discounts.Models.Responses;
using RCS.API.Discounts.Repository;

namespace RCS.API.Discounts.Queries;

public record GetAllDiscountsQuery : IRequest<IEnumerable<DiscountResponse>>;

public class GetAllDiscountsHandler : IRequestHandler<GetAllDiscountsQuery, IEnumerable<DiscountResponse>>
{
    private readonly IDiscountRepository _discountRepository;

    public GetAllDiscountsHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<IEnumerable<DiscountResponse>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
    {
        var discountList = await _discountRepository.GetAllDiscountsAsync();
        return discountList.Select(d => new DiscountResponse(d.Id, d.Name, d.Value, d.StartDate, d.EndDate));
    }
}
