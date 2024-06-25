using RCS.API.Data.Models;

namespace RCS.API.Discounts.Repository;

public interface IDiscountRepository
{
    Task<int> CreateDiscountAsync(Discount discount);
    Task<IEnumerable<Discount>> GetAllDiscountsAsync();
}
