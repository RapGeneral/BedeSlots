using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.GlobalData.MappingProvider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services
{
    public class UserBankDetailsServices : IUserBankDetailsServices
    {
        private readonly IRepository<UserBankDetails> userBankDetailsRepo;
        private readonly IMappingProvider mappingProvider;

        public UserBankDetailsServices(IMappingProvider mappingProvider, IRepository<UserBankDetails> userBankDetailsRepo)
        {
            this.userBankDetailsRepo = userBankDetailsRepo;
            this.mappingProvider = mappingProvider;
        }

        public async Task<BankDetailsViewModel> DeleteUserBankDetailsAsync(string bankDetailsId, string userId)
        {
            if(userId is null)
            {
                throw new ArgumentNullException("UserId cannot be null!");
            }
            if(bankDetailsId is null)
            {
                throw new ArgumentNullException("BankDetailsId cannot be null!");
            }
            var userBankDetailsToRemove = await userBankDetailsRepo.All()
                .Include(ubd => ubd.User)
                .Include(ubd => ubd.BankDetails)
                .Where(ubd => ubd.UserId == userId
                    && ubd.BankDetailsId.ToString() == bankDetailsId)
                .FirstOrDefaultAsync();

            if(userBankDetailsToRemove is null)
            {
                throw new ArgumentException("UserBankDetails not found!");
            }

            var bankDetails = userBankDetailsToRemove.BankDetails;
            userBankDetailsToRemove.IsDeleted = true;
            await userBankDetailsRepo.SaveAsync();

            return mappingProvider.MapTo<BankDetailsViewModel>(bankDetails);
        }
    }
}
