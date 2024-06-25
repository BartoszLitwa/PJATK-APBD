using RCS.API.Data.Models;

namespace RCS.API.Softwares.Repository;

public interface ICurrencyRepository
{
    Task<Currency?> AddCurrencyAsync(Currency? currency);
    Task<Currency?> GetCurrencyAsync(string code);
    Task<IEnumerable<Currency?>> GetAllCurrenciesAsync();
    Task UpdateCurrencyAsync(Currency? currency);
}
