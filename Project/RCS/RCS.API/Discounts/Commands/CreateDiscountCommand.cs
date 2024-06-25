using MediatR;
using RCS.API.Data.Models;
using RCS.API.Discounts.Models.Requests;
using RCS.API.Discounts.Models.Responses;
using RCS.API.Discounts.Repository;

namespace RCS.API.Discounts.Commands;

public record CreateDiscountCommand(CreateDiscountRequest Request) : IRequest<CreateDiscountResponse>;

public class CreateDiscountHandler(IDiscountRepository _discountRepository)
    : IRequestHandler<CreateDiscountCommand, CreateDiscountResponse>
{
    public async Task<CreateDiscountResponse> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = new Discount
        {
            Name = request.Request.Name,
            Value = request.Request.Value,
            StartDate = request.Request.StartDate,
            EndDate = request.Request.EndDate
        };

        var discountId = await _discountRepository.CreateDiscountAsync(discount);
        return new CreateDiscountResponse(discountId, discount.Name);
    }
}
