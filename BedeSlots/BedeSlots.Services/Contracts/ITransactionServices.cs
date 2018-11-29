using BedeSlots.ViewModels.Enums;
using BedeSlots.ViewModels.GlobalViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface ITransactionServices
    {
        Task<TransactionViewModel> CreateTransactionAsync(TypeOfTransaction type, string description, decimal amount, string userId);
        Task<ICollection<TransactionViewModel>> SearchTransactionAsync(string username, int? min, int? max, ICollection<string> types);
    }
}