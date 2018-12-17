using BedeSlots.ViewModels.GlobalViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface IUserServices
    {
        Task<ICollection<UserViewModel>> SearchByUsernameAsync(string username);
        /// <summary>
        /// Updates the user's balances, returning the change in USD
        /// </summary>
        /// <param name="nativeMoney">The difference in user's native currency</param>
        /// <returns></returns>
        Task<decimal> UpdateUserBalanceByAmount(decimal nativeMoney, string userId);
        /// <summary>
        /// Create the balances the user would need when registered.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateUserInitialBalances(string userId, string nativeCurrency);
        Task<MoneyViewModel> GetBalanceInformation(string userId);
        Task<ICollection<BankDetailsViewModel>> GetBankDetailsInformation(string userId);
    }
}
