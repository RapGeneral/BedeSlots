using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly IRepository<Currency> currencyRepo;        

        public CurrencyServices(IRepository<Currency> currencyRepo)
        {
            this.currencyRepo = currencyRepo;
        }

        public async Task<List<string>> GetCurrenciesAsync()
        {
            var currencyNames = await currencyRepo.All().Select(c => c.CurrencyName).ToListAsync();

            return currencyNames;
        }
    }
}
