using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.GlobalViewModels;
using System;
using System.Threading.Tasks;
using BedeSlots.Infrastructure.MappingProvider;

namespace BedeSlots.Services
{
    public class BankDetailsService : IBankDetailsService
    {
        private readonly IRepository<BankDetails> bankDetailsRepo;
        private readonly IMappingProvider mappingProvider;

        public BankDetailsService(IRepository<BankDetails> bankDetailsRepo, IMappingProvider mappingProvider)
        {
            this.bankDetailsRepo = bankDetailsRepo;
            this.mappingProvider = mappingProvider;
        }

        public async Task<BankDetailsViewModel> AddBankDetailsAsync(string number, int cvv, DateTime expiryDate)
        {
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
            await bankDetailsRepo.SaveAsync();

            var model = mappingProvider.MapTo<BankDetailsViewModel>(bankDetails);          
            return model;
        }

        public async Task<BankDetailsViewModel> DeleteBankDetailsAsync(Guid Id)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}