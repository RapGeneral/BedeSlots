using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.GlobalViewModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BedeSlots.Services
{
    public class BankDetailsService : IBankDetailsService
    {
        private readonly IRepository<BankDetails> bankDetailsRepo;

        public BankDetailsService(IRepository<BankDetails> bankDetailsRepo)
        {
            this.bankDetailsRepo = bankDetailsRepo;
        }

        public async Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate)
        {
            //formating to be Month/year ,,, to be here or smwh else??
            DateTime now = DateTime.Now;
            expiryDate.ToString("MM/yyyy");
            now.ToString("MM/yyyy");

            int dateResult = DateTime.Compare(expiryDate, now);
            
            if (dateResult < 0)
            {
                throw new ArgumentOutOfRangeException("The card is expired!");
            }

            var bankDetails = new BankDetails
            {
                Number = number,
                Cvv = cvv,
                ExpiryDate = expiryDate,
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };

            bankDetailsRepo.Add(bankDetails);
            await bankDetailsRepo.SaveAsync();

            var model = new BankDetailsViewModel(bankDetails);            
            return model;
        }

        public async Task<BankDetailsViewModel> DeleteBankDetailsAsync(Guid Id)
        {
            
            
        }
    }
}
