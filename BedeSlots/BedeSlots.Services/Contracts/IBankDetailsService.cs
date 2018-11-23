using BedeSlots.DataModels;
using BedeSlots.ViewModels.GlobalViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface IBankDetailsService
    {
        Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate);

        Task<BankDetailsViewModel> DeleteBankDetailsAsync(Guid Id);
    }
}