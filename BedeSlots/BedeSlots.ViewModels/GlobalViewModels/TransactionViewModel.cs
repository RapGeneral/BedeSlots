using BedeSlots.DataModels;
using BedeSlots.ViewModels.Enums;
using System;

namespace BedeSlots.ViewModels
{
    public class TransactionViewModel
    {
        public TransactionViewModel(Transaction transaction)
        {
            Date = transaction.Date;
            Type = Enum.Parse<TypeOfTransaction>(transaction.Type.Name, true);
            Amount = transaction.Amount;
            Description = transaction.Description;
            Username = transaction.Balance.User.UserName;
        }
        public DateTime Date { get; set; }

        public TypeOfTransaction Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string Username { get; set; }

    }
}