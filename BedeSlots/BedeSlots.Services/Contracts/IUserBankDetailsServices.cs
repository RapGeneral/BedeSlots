using BedeSlots.GlobalData.GlobalViewModels;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface IUserBankDetailsServices
    {
        Task<BankDetailsViewModel> DeleteUserBankDetailsAsync(string bankDetailsId, string userId);
    }
}
