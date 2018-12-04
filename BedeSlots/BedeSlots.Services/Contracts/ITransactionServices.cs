using BedeSlots.ViewModels;
using BedeSlots.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface ITransactionServices
    {
        Task<TransactionViewModel> CreateTransactionAsync(TypeOfTransaction type, string description, decimal amount, string userId);
        Task<ICollection<TransactionViewModel>> SearchTransactionAsync(string username, int? min, int? max, ICollection<string> types);
    }
}