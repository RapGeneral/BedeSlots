using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface ICurrencyServices
    {
        Task<ICollection<string>> GetCurrenciesAsync();
    }
}
