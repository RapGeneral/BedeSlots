using BedeSlots.GlobalData.Enums;
using BedeSlots.GlobalData.GlobalViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface ITransactionServices
    {
        Task<TransactionViewModel> CreateTransactionAsync(TypeOfTransaction type, string description, decimal amount, string userId);
        Task<ICollection<TransactionViewModel>> SearchTransactionAsync(string username, int? min, int? max, ICollection<string> types, string sortProp, bool descending = false);
        Task<ICollection<string>> GetTypesAsync();
    }
}