using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services;
using BedeSlots.GlobalData.Enums;
using BedeSlots.GlobalData.MappingProvider;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Services.UserServicesTests
{
    [TestClass]
    public class CreateUserInitialBalancesShould
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenUserIdIsNull()
        {
            //Arrange
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var userRepoMock = new Mock<IRepository<User>>();
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.CreateUserInitialBalances(null, "notNull"));
        }
        [TestMethod]
        public async Task CreateTwoUserBalances()
        {
            //Arrange
            const string userId = "poesho";
            const string nativeCurrency = "EUR";
            const string baseCurrency = "USD";
			Guid baseTypeId = Guid.NewGuid();
			Guid nativeTypeId = Guid.NewGuid();
			var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var currency1 = new Currency { CurrencyName = baseCurrency };
            var currency2 = new Currency { CurrencyName = nativeCurrency };
            var currency3 = new Currency { CurrencyName = "BGN" };
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            currencyRepoMock
                .Setup(crm => crm.All())
                .Returns(new List<Currency> { currency1, currency2, currency3 }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance>()
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);
            var newlyCreatedBalances = new List<Balance>();
            balanceRepoMock
                .Setup(brm => brm.AddAsync(It.IsAny<Balance>()))
                .Callback<Balance>(newBal => newlyCreatedBalances.Add(newBal))
                .Returns(Task.CompletedTask);

            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();

            var balanceTypeBase = new BalanceType { Name = "Base", Id = baseTypeId };
            var balanceTypeNative = new BalanceType { Name = "Personal", Id = nativeTypeId };
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            balanceTypeRepo
                .Setup(btr => btr.All())
                .Returns(new List<BalanceType> { balanceTypeBase, balanceTypeNative }
                            .AsQueryable()
                            .BuildMock()
                            .Object);

            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act
            await sut.CreateUserInitialBalances(userId, nativeCurrency);
            //Assert
            balanceRepoMock.Verify(brm => brm.AddAsync(It.IsAny<Balance>()), Times.Exactly(2));
            balanceRepoMock.Verify(brm => brm.SaveAsync(), Times.Once);
            Assert.IsTrue(newlyCreatedBalances.Any(ncb => ncb.Currency.CurrencyName == nativeCurrency && ncb.TypeID ==nativeTypeId));
            Assert.IsTrue(newlyCreatedBalances.Any(ncb => ncb.Currency.CurrencyName == baseCurrency && ncb.TypeID == baseTypeId));
            Assert.IsTrue(newlyCreatedBalances.All(ncb => ncb.UserId == userId));
        }
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenCurrencyIsNull()
        {
            //Arrange
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var newlyCreatedBalances = new List<Balance>();
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.CreateUserInitialBalances("awdawd", null));
        }
        [TestMethod]
        public async Task ThrowsArgumentException_WhenUserAlreadyHasBalances()
        {
            //Arrange
            const string userId = "pesho";
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance> { new Balance { UserId = userId } }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserInitialBalances(userId, "randomCur"));
        }
        [DataTestMethod]
        [DataRow("EUR", "daw", "EUR")]
        [DataRow("EUR", "USD", "daw")]
        public async Task ThrowsArgumentException_WhenCurrenciesIncorrectlyFound(string searchedCurrency, string actualBaseCurrency, string actualNativeCurrency)
        {
            //Arrange
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var currency1 = new Currency { CurrencyName = actualBaseCurrency };
            var currency2 = new Currency { CurrencyName = actualNativeCurrency };
            var currency3 = new Currency { CurrencyName = "BGN" };
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            currencyRepoMock
                .Setup(crm => crm.All())
                .Returns(new List<Currency> { currency1, currency2, currency3 }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance>()
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserInitialBalances("randomuserid", searchedCurrency));
        }
    }
}
