using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Data.Models;
using webapi.IRepository;
using webapi.Models.DTO;

namespace webapi.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly EFDBContext _context;

        public CurrencyRepository(EFDBContext context)
        {
            _context = context;
        }

        public async Task<List<Currency>> Gets()
        {
            return await _context.Currencies.AsNoTracking().Where(x => x.Cur_ID == 431 || x.Cur_ID == 451 || x.Cur_ID == 456 || x.Cur_ID == 292 || x.Cur_ID == 999).ToListAsync();
        }

        public async Task Save(List<Currency> obj)
        {
            var commitCurrency = await _context.Currencies.ToListAsync();
            if (commitCurrency.Count() != 0)
            {
                foreach (var item in commitCurrency)
                {
                    var modify = obj.FirstOrDefault(x => x.Cur_ID == item.Cur_ID);
                    if (modify != null)
                    {
                        item.Date = modify.Date;
                        item.Cur_Scale = modify.Cur_Scale;
                        item.Cur_OfficialRate = modify.Cur_OfficialRate;
                        item.UpdateDataInDb = DateTime.Now;
                        _context.Entry(item).State = EntityState.Modified;
                    }
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.Currencies.AddRangeAsync(obj);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<CurrencyChange>> GetByCurrencyChangesByUserId(Guid userId)
        {
            if (userId != Guid.Empty)
                return await _context.CurrencyChanges.AsNoTracking().Where(x => x.UserId == userId).OrderBy(x => x.TimeStamp).ToListAsync();
            else return new List<CurrencyChange>();

        }

        public async Task<List<CurrencyChange>> SaveCurrencyChange(CurrencyChangeDTO currencyChange)
        {
            var modelToDb = new CurrencyChange
            {
                FirstCurrency = currencyChange.FirstCurrency,
                FirstTotal = currencyChange.FirstTotal,
                Id = currencyChange.Id,
                SecoundCurrency = currencyChange.SecoundCurrency,
                SecoundTotal = currencyChange.SecoundTotal,
                UserId = currencyChange.UserId,
                TimeStamp = DateTime.Now
            };

            await _context.CurrencyChanges.AddAsync(modelToDb);
            await _context.SaveChangesAsync();
            return await GetByCurrencyChangesByUserId(currencyChange.UserId);
        }
    }
}
