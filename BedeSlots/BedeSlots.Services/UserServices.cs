using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.MappingProvider;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.GlobalViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.GlobalData.Enums;

namespace BedeSlots.Services
{
    public class UserServices : IUserServices
    {
        private readonly IRepository<BalanceType> balanceTypeRepo;
        private readonly IMappingProvider mappingProvider;
        private readonly IRepository<User> userRepo;
        private readonly IRepository<Currency> currencyRepo;
        private readonly IRepository<Balance> balanceRepo;
        private readonly IMemoryCache cache;
        private readonly IRepository<UserBankDetails> userBankDetailsRepo;
        private const string BASE_CURRENCY = "USD";
        public UserServices(
            IRepository<User> userRepo, 
            IMappingProvider mappingProvider, 
            IMemoryCache cache, 
            IRepository<Currency> currencyRepo, 
            IRepository<Balance> balanceRepo,
            IRepository<UserBankDetails> userBankDetailsRepo,
            IRepository<BalanceType> balanceTypeRepo)
        {
            this.balanceTypeRepo = balanceTypeRepo;
            this.mappingProvider = mappingProvider;
            this.userRepo = userRepo;
            this.currencyRepo = currencyRepo;
            this.balanceRepo = balanceRepo;
            this.cache = cache;
            this.userBankDetailsRepo = userBankDetailsRepo;
        }

        public async Task<ICollection<UserViewModel>> SearchByUsernameAsync(string username)
        {
            List<User> users;
            if (username is null)
            {
                users = await userRepo.All().ToListAsync();
            }
            else
            {
                users = await userRepo.All().Where(u => u.UserName.ToLower()
                                    .Contains(username.ToLower()))
                                    .ToListAsync();
            }
            var models = mappingProvider.MapTo<ICollection<UserViewModel>>(users);
            return models;
        }

        public async Task<decimal> UpdateUserBalanceByAmount(decimal nativeMoney, string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("UserId cannot be null!");
            }
            var userBalances = await balanceRepo.All()
                                        .Where(b => b.UserId == userId)
                                        .Include(b => b.Currency)
                                        .Include(b => b.Type)
                                        .ToListAsync();
            if(userBalances.Count < 1)
            {
                throw new ArgumentException("The user does not have enough balances!");
            }

            var nativeBalance = userBalances.Where(b => b.Type.Name == BalanceTypes.Personal.ToString()).First();
            var baseBalance = userBalances.Where(b => b.Type.Name == BalanceTypes.Base.ToString()).First();

            var rate = await GetCurrencyRateChached(nativeBalance.Currency.CurrencyName);
            var moneyInBaseCurrency = nativeMoney * (1 / rate);

            nativeBalance.Money += nativeMoney;
            baseBalance.Money += moneyInBaseCurrency;
            await balanceRepo.SaveAsync();
            return moneyInBaseCurrency;
        }

        public async Task CreateUserInitialBalances(string userId, string nativeCurrency)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("UserId cannot be null!");
            }
            if (string.IsNullOrEmpty(nativeCurrency))
            {
                throw new ArgumentNullException("Cannot create user balances with null native currency!");
            }

            var balances = await balanceRepo.All().Where(bal => bal.UserId == userId).ToListAsync();

            if (balances.Count != 0)
            {
                throw new ArgumentException("User already has balances on his id!");
            }

            var currencies = await currencyRepo.All()
                .Where(cur => cur.CurrencyName.ToLower() == nativeCurrency.ToLower() 
                           || cur.CurrencyName.ToLower() == BASE_CURRENCY.ToLower())
                .ToListAsync();
            if(currencies.Count != 2)
            {
                throw new ArgumentException("Found currencies are not 2!");
            }

            var nativeBalance = new Balance
            {
                UserId = userId,
                Currency = currencies.First(cur => cur.CurrencyName.ToLower() == nativeCurrency.ToLower()),
                Type = await GetNativeBalanceTypeCached()
            };
            var baseBalance = new Balance
            {
                UserId = userId,
                Currency = currencies.First(cur => cur.CurrencyName.ToLower() == BASE_CURRENCY.ToLower()),
                Type = await GetBaseBalanceTypeCached()
            };

            await balanceRepo.AddAsync(nativeBalance);
            await balanceRepo.AddAsync(baseBalance);
            await balanceRepo.SaveAsync();
        }

        public async Task<MoneyViewModel> GetBalanceInformation(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("UserId cannot be null!");
            }
            var userBalance = await balanceRepo.All()
                .Include(bal => bal.Currency)
                .Include(bal => bal.Type)
                .Where(bal => bal.UserId == userId 
                    && bal.Type.Name == BalanceTypes.Personal.ToString())
                .FirstOrDefaultAsync();

            if(userBalance is null)
            {
                throw new ArgumentException("User balance is not found!");
            }
            var model = mappingProvider.MapTo<MoneyViewModel>(userBalance);
            return model;
        }

        public async Task<ICollection<BankDetailsViewModel>> GetBankDetailsInformation(string userId)
        {
            if(userId is null)
            {
                throw new ArgumentNullException("UserId cannot be null!");
            }
            var userBankDetails = await userBankDetailsRepo.All()
                                    .Where(bd => bd.UserId == userId && !bd.IsDeleted)
                                    .Select(ubd => ubd.BankDetails)
                                    .ToListAsync();

            var models = mappingProvider.MapTo<ICollection<BankDetailsViewModel>>(userBankDetails);
            return models;
        }

        private async Task<BalanceType> GetNativeBalanceTypeCached()
        {
            return await cache.GetOrCreate("BalanceTypeNative", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                return await balanceTypeRepo.All().Where(bal => bal.Name == BalanceTypes.Personal.ToString()).FirstOrDefaultAsync();
            });
        }
        private async Task<BalanceType> GetBaseBalanceTypeCached()
        {
            return await cache.GetOrCreate("BalanceTypeBase", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                return await balanceTypeRepo.All().Where(bal => bal.Name == BalanceTypes.Base.ToString()).FirstOrDefaultAsync();
            });
        }

        private async Task<decimal> GetCurrencyRateChached(string currency)
        {
            if(currency is null)
            {
                throw new ArgumentNullException("Cannot get the currency rate from null!");
            }

            var currencyRatesCached = await cache.GetOrCreate("CurrencyRates", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return await currencyRepo
                 .All()
                 .Include(cur => cur.Rates)
                 .Select(curs => new {
                     Name = curs.CurrencyName,
                     Rate = curs.Rates.OrderByDescending(rate => rate.CreatedAt).FirstOrDefault().Coeff
                 })
                 .ToListAsync();
            });
            return currencyRatesCached.FirstOrDefault(curRate => curRate.Name.ToLower() == currency.ToLower()).Rate;
        }

    }
}
