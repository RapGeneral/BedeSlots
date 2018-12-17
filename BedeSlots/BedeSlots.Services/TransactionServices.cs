using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.Enums;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.GlobalData.MappingProvider;
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
            var balance = await balanceRepo.All()
                .Include(b => b.User)
                .Include(b => b.Type)
                .Where(b => b.UserId == userId && b.Type.Name == BalanceTypes.Base.ToString())
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
                OpeningBalance = balance.Money - amount
            };

            transactionRepo.Add(transaction);
            await transactionRepo.SaveAsync();

            var model = mappingProvider.MapTo<TransactionViewModel>(transaction);

            return model;
        }

        public async Task<ICollection<TransactionViewModel>> SearchTransactionAsync(string username, int? min, int? max, ICollection<string> types, string sortProp, bool descending = false)
        {
            IQueryable<Transaction> transactions = transactionRepo.All()
                .Include(tr => tr.Balance)
                    .ThenInclude(b => b.User)
                .Include(tr => tr.Type);

            if (username != null)
            {
                transactions = transactions.Where(tr => tr.Balance.User.UserName.ToLower().Contains(username.ToLower()));
            }

            if (min == null && max != null)
            {
                transactions = transactions.Where(tr => Math.Abs(tr.Amount) < (decimal)max);
            }
            if (max == null && min != null)
            {
                transactions = transactions.Where(tr => Math.Abs(tr.Amount) > (decimal)min);
            }
            if (min != null && max != null)
            {
                if (max < min)
                {
                    throw new ArgumentOutOfRangeException("Max value can`t be greater than min value!");
                }
                transactions = transactions.Where(tr => tr.Amount > min && tr.Amount < max);
            }


            if (!(types is null) && types.Count != 0)
            {
                transactions = transactions.Where(tr => types.Any(type => tr.Type.Name.ToLower() == type.ToLower()));
            }

            if (!(sortProp is null))
            {
                if (descending)
                {
                    if (sortProp == "Username")
                    {
                        transactions = transactions.OrderByDescending(tr => tr.Balance.User);
                    }
                    else
                    {
                        transactions = transactions.OrderByDescending(tr => tr.GetType().GetProperty(sortProp).GetValue(tr, null));
                    }
                }
                else
                {
                    if (sortProp == "Username")
                    {
                        transactions = transactions.OrderByDescending(tr => tr.Balance.User);
                    }
                    else
                    {
                        transactions = transactions.OrderBy(tr => tr.GetType().GetProperty(sortProp).GetValue(tr, null));
                    }
                }
            }

            var foundTrnasaciton = await transactions.ToListAsync();

            return mappingProvider.MapTo<ICollection<TransactionViewModel>>(foundTrnasaciton);
        }

        public async Task<ICollection<string>> GetTypesAsync()
        {
            return await transactionTypeRepo.All().Select(trt => trt.Name).ToListAsync();
        }
    }
}