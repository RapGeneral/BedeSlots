using BedeSlots.ViewModels.GlobalViewModels;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface IBankDetailsServices
    {
        Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate, string userId);
    }
}