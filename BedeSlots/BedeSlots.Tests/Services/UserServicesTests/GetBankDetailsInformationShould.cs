using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.ViewModels.MappingProvider;
using BedeSlots.Services;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Services.UserServicesTests
{
    [TestClass]
    public class GetBankDetailsInformationShould
    {
        [TestMethod]
        public async Task CreateTwoUserBalances()
        {
            //Arrange
            const string userId = "poesho";

            var mapInputList = new List<BankDetails>();
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<BankDetailsViewModel>>(It.IsAny<List<BankDetails>>()))
                .Callback<object>(inpt => mapInputList = inpt as List<BankDetails>);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            var balanceRepoMock = new Mock<IRepository<Balance>>();

            var bankDetails1 = new BankDetails();
            var bankDetails2 = new BankDetails();
            var userBankDetails1 = new UserBankDetails {UserId = userId, BankDetails = bankDetails1 };
            var userBankDetails2 = new UserBankDetails {UserId = userId, BankDetails = bankDetails2 };
            var userBankDetails3 = new UserBankDetails {UserId = "not" + userId };
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            userBankDetailsMock
                .Setup(ubdm => ubdm.All())
                .Returns(new List<UserBankDetails> {userBankDetails1, userBankDetails2, userBankDetails3 }
                                .AsQueryable()
                                .BuildMock()
                                .Object);

            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act
            await sut.GetBankDetailsInformation(userId);
            //Assert
            Assert.IsTrue(mapInputList.Any(bankD => bankD.Equals(bankDetails1)));
            Assert.IsTrue(mapInputList.Any(bankD => bankD.Equals(bankDetails2)));
        }
    }
}
