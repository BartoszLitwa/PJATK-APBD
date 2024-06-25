using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Discounts.Repository;

public class DiscountRepository(RcsDbContext _context) : IDiscountRepository
{
    public async Task<int> CreateDiscountAsync(Discount discount)
    {
        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();
        return discount.Id;
    }

    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
    {
        return await _context.Discounts.ToListAsync();
    }
}