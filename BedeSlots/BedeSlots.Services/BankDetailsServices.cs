using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.GlobalViewModels;
using System;
using System.Threading.Tasks;
using BedeSlots.Infrastructure.MappingProvider;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BedeSlots.Services
{
    public class BankDetailsServices : IBankDetailsServices
    {
        private readonly IRepository<BankDetails> bankDetailsRepo;
        private readonly IMappingProvider mappingProvider;

        public BankDetailsServices(IRepository<BankDetails> bankDetailsRepo, IMappingProvider mappingProvider)
        {
            this.bankDetailsRepo = bankDetailsRepo;
            this.mappingProvider = mappingProvider;
        }

        public async Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate, string userId)
        {
            var existingBankDetails = await bankDetailsRepo.All().Where(bd => bd.Number == number && bd.Cvv == cvv).ToListAsync();
            if (!(existingBankDetails is null))
            {
                throw new ArgumentNullException("The card already exists!");
            }
            
            int dateResult = DateTime.Compare(expiryDate, DateTime.Now);
            
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
            var userBankDetails = new List<UserBankDetails> { new UserBankDetails { UserId = userId, BankDetailsId = bankDetails.Id } };
            bankDetails.UserBankDetails = userBankDetails;
            await bankDetailsRepo.SaveAsync();

            var model = mappingProvider.MapTo<BankDetailsViewModel>(bankDetails);          
            return model;
        }

        public async Task DeleteBankDetailsAsync(Guid Id)
        {
            var cardToRemove = bankDetailsRepo.All().Where(ctr => ctr.Id == Id).FirstOrDefault();

            cardToRemove.IsDeleted = true;

            await bankDetailsRepo.SaveAsync();
        }
    }
}