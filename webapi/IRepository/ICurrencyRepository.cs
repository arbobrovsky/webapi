using webapi.Data.Models;
using webapi.Models.DTO;

namespace webapi.IRepository
{
    public interface ICurrencyRepository
    {
        Task Save(List<Currency> obj);
        Task<List<Currency>> Gets();
        Task<List<CurrencyChange>> SaveCurrencyChange(CurrencyChangeDTO currencyChange);
        Task<List<CurrencyChange>> GetByCurrencyChangesByUserId(Guid userId);
        
    }
}
