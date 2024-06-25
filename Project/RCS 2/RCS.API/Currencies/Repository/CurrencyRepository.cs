using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Softwares.Repository;

public class CurrencyRepository(RcsDbContext _context) : ICurrencyRepository
{
    public async Task<Currency?> AddCurrencyAsync(Currency? currency)
    {
        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
        return currency;
    }

    public async Task<Currency?> GetCurrencyAsync(string code)
    {
        return await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<IEnumerable<Currency?>> GetAllCurrenciesAsync()
    {
        return await _context.Currencies.ToListAsync();
    }

    public async Task UpdateCurrencyAsync(Currency? currency)
    {
        _context.Currencies.Update(currency);
        await _context.SaveChangesAsync();
    }
}
