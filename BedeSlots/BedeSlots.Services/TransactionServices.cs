using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.MappingProvider;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels;
using BedeSlots.ViewModels.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IRepository<Balance> balanceRepo;
        private readonly IRepository<Transaction> transactionRepo;
        private readonly IRepository<TransactionType> transactionTypeRepo;
        private readonly IMappingProvider mappingProvider;

        public TransactionServices(IRepository<Balance> balanceRepo, IRepository<Transaction> transactionRepo, IRepository<TransactionType> transactionTypeRepo, IMappingProvider mappingProvider)
        {
            this.balanceRepo = balanceRepo;
            this.transactionRepo = transactionRepo;
            this.transactionTypeRepo = transactionTypeRepo;
            this.mappingProvider = mappingProvider;
        }

        public async Task<TransactionViewModel> CreateTransactionAsync(TypeOfTransaction type, string description, decimal amount, string userId)
        {
            //fix the magic string when we have base update
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

            var model = mappingProvider.MapTo<TransactionViewModel>(transaction);

            return model;
        }

        public async Task<ICollection<TransactionViewModel>> SearchTransactionAsync(string username, int min, int max, ICollection<string> types)
        {
            IQueryable<Transaction> transactions = transactionRepo.All()
                .Include(tr => tr.Amount)
                .Include(tr => tr.Balance)
                    .ThenInclude(b => b.User)
                .Include(tr => tr.Type)
                    .ThenInclude(ty => ty.Name);

            if (username != null)
            {
                transactions = transactions.Where(tr => tr.Balance.User.UserName.ToLower().Contains(username.ToLower()));
            }

            if (max < min)
            {
                throw new ArgumentOutOfRangeException("Max value must be greater than 0");
            }
            transactions = transactions.Where(tr => tr.Amount > min && tr.Amount < max);

            if (types.Count == 0)
            {
                transactions = transactions.Where(tr => types.Any(type => tr.Type.Name.ToLower() == type.ToLower()));
            }

            var foundedTrnasaciton = await transactions.ToListAsync();

            return mappingProvider.MapTo<ICollection<TransactionViewModel>>(foundedTrnasaciton);
        }
    }
}