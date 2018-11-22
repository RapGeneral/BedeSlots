using BedeSlots.DataContext;
using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels;
using BedeSlots.ViewModels.Enums;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Balance> balanceRepo;
        private readonly IRepository<Transaction> transactionRepo;
        private readonly IRepository<TransactionType> transactionTypeRepo;

        public TransactionService(IRepository<Balance> balanceRepo, IRepository<Transaction> transactionRepo, IRepository<TransactionType> transactionTypeRepo)
        {
            this.balanceRepo = balanceRepo;
            this.transactionRepo = transactionRepo;
            this.transactionTypeRepo = transactionTypeRepo;
        }

        public async Task<TransactionViewModel> CreateTransactionAsync(TypeOfTransaction type, string description, decimal amount, string userId)
        {
            var balance = await balanceRepo.All()
                .Include(b => b.User)
                .Include(b => b.Currency)
                .Where(b => b.UserId == userId && b.Currency.CurrencyName != "USD")
                .FirstOrDefaultAsync();

            var transactionType = await transactionTypeRepo.All()
                .Where(t => t.Name.ToLower() == type.ToString().ToLower())
                .FirstOrDefaultAsync();

            var transaction = new Transaction
            {
                Type = transactionType,
                Balance = balance,
                Date = DateTime.Now,
                Description = description,
                Amount = amount,
                OpeningBalance = balance.Money                
            };

            transactionRepo.Add(transaction);
            await transactionRepo.SaveAsync();

            var model = new TransactionViewModel(transaction);
            return model;
        }

        public async Task<ICollection<TransactionViewModel>> SearchTransactionAsync(string username, int min, int max, ICollection<string> types)
        {
            IQueryable<TransactionViewModel> transactions = transactionRepo.All()
                .Include(tr => tr.Amount)
                .Include(tr => tr.Type)
                .ThenInclude(ty => ty.Name)
                .Select(tr => new TransactionViewModel(tr));

            if (username != null)
            {
                transactions = transactions.Where(tr => tr.Username.Contains(username));
            }

            if (min > 0)
            {
                if (max == 0)
                {
                    throw new ArgumentOutOfRangeException("Max value must be greater than 0");
                }
                transactions = transactions.Where(tr => tr.Amount > min && tr.Amount < max);
            }

            if (!(types.First() == null))
            {
                transactions = transactions.Where(tr => tr.Type.ToString() == types.ToString());
            }            
            
            var findedTransactions = await transactions.ToListAsync();

            return findedTransactions;
        }
    }
}
