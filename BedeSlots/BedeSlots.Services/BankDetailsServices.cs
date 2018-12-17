using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.GlobalViewModels;
using BedeSlots.ViewModels.MappingProvider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services
{
    public class BankDetailsServices : IBankDetailsServices
    {
        private readonly IRepository<UserBankDetails> userBankDetailsRepo;
        private readonly IRepository<BankDetails> bankDetailsRepo;
        private readonly IMappingProvider mappingProvider;

        public BankDetailsServices(IRepository<BankDetails> bankDetailsRepo, IMappingProvider mappingProvider, IRepository<UserBankDetails> userBankDetailsRepo)
        {
            this.userBankDetailsRepo = userBankDetailsRepo;
            this.bankDetailsRepo = bankDetailsRepo;
            this.mappingProvider = mappingProvider;
        }

        public async Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate, string userId)
        {
            //TODO add check if exists; If exists it should throw custom error
            //Hook them to a user
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
                IsDeleted = false,
                UserBankDetails = new List<UserBankDetails>()
            };

            bankDetailsRepo.Add(bankDetails);
            var userBankDetails = new UserBankDetails { UserId = userId, BankDetailsId = bankDetails.Id };
            bankDetails.UserBankDetails.Add(userBankDetails);
            await bankDetailsRepo.SaveAsync();

            var model = mappingProvider.MapTo<BankDetailsViewModel>(bankDetails);
            return model;
        }
    }
}