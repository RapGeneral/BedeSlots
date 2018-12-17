using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.GlobalData.MappingProvider;
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
        private readonly IDateTimeWrapper dateTime;

        public BankDetailsServices(IRepository<BankDetails> bankDetailsRepo, IMappingProvider mappingProvider, IRepository<UserBankDetails> userBankDetailsRepo, IDateTimeWrapper dateTime)        
        {
            this.userBankDetailsRepo = userBankDetailsRepo;
            this.bankDetailsRepo = bankDetailsRepo;
            this.mappingProvider = mappingProvider;
            this.dateTime = dateTime;
        }

        public async Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate, string userId)
        {
            var existingBankDetails = await bankDetailsRepo.All().Where(bd => bd.Number == number && bd.Cvv == cvv).ToListAsync();

            if (existingBankDetails.Count != 0)
            {
                throw new ArgumentNullException("The card already exists!");
            }
            
            int dateResult = DateTime.Compare(expiryDate, dateTime.Now());
            
            if (dateResult < 0)
            {
                throw new ArgumentOutOfRangeException("The card is expired!");
            }

            var potentialBankDetails = await bankDetailsRepo.All().Where(bd => bd.Number == number).FirstOrDefaultAsync();
            if(!(potentialBankDetails is null))
            {
                var potentialUserBankDetails = await userBankDetailsRepo.All()
                                        .Where(ubd => ubd.UserId == userId 
                                            && ubd.BankDetailsId == potentialBankDetails.Id)
                                        .FirstOrDefaultAsync();
                if (potentialUserBankDetails.IsDeleted)
                {
                    potentialUserBankDetails.IsDeleted = false;
                    await userBankDetailsRepo.SaveAsync();
                    var modelToReturn = mappingProvider.MapTo<BankDetailsViewModel>(potentialBankDetails);
                    return modelToReturn;
                }
                else
                {
                    throw new ArgumentException("Bank details already exists and it is connected to the user!");
                }
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