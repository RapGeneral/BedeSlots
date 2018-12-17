using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.ViewModels.MappingProvider;
using BedeSlots.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.ViewModels.Enums;

namespace BedeSlots.Tests.Services.UserServicesTests
{
    [TestClass]
    public class UpdateUserBalanceByAmountShould
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
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.UpdateUserBalanceByAmount(102, null));
        }
        [DataTestMethod]
        [DataRow(10)]
        [DataRow(-10)]
        [DataRow(0)]
        public async Task UpdateTheTwoPlayerBalances(int moneyToAddInNative)
        {
            //Arrange
            const string userId1 = "Pesho";
            const string userId2 = "somethingRandom";
            const decimal balance1OpeningMoney = 50; //thats USD
            const decimal balance12OpeningMoney = 100; //thats EUR

            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var currency1 = new Currency { CurrencyName = "USD" };
            var currency2 = new Currency { CurrencyName = "EUR" };
            var currency3 = new Currency { CurrencyName = "BGN" };
            var rate1 = new Rate {BaseCurrency = currency1, ToCurrency = currency2, Coeff = 58, CreatedAt = DateTime.Now.AddYears(-1) };
            var rate12 = new Rate {BaseCurrency = currency1, ToCurrency = currency2, Coeff = 2, CreatedAt = DateTime.Now.AddYears(-1) };
            var rate2 = new Rate {BaseCurrency = currency1, ToCurrency = currency3, Coeff = 23141, CreatedAt = DateTime.Now.AddYears(-1) };
            var rate22 = new Rate {BaseCurrency = currency1, ToCurrency = currency3, Coeff = 0.5M, CreatedAt = DateTime.Now.AddYears(-1) };
            currency2.Rates = new List<Rate> { rate1, rate12 };
            currency3.Rates = new List<Rate> { rate2, rate22 };
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            currencyRepoMock
                .Setup(crm => crm.All())
                .Returns(new List<Currency> { currency2, currency3 }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            
            var balance1 = new Balance { Currency = currency1, UserId = userId1, Money = balance1OpeningMoney, Type = new BalanceType { Name = BalanceTypes.Base.ToString() } }; //Thats USD
            var balance12 = new Balance { Currency = currency2, UserId = userId1, Money = balance12OpeningMoney, Type = new BalanceType { Name = BalanceTypes.Personal.ToString() } };//Thats EUR
            var balance2 = new Balance { Currency = currency1, UserId = userId2, Type = new BalanceType { Name = BalanceTypes.Base.ToString() } }; 
            var balance22 = new Balance { Currency = currency3, UserId = userId2, Type = new BalanceType { Name = BalanceTypes.Personal.ToString() } };
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance> { balance1, balance12 }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act
            var result = await sut.UpdateUserBalanceByAmount(moneyToAddInNative, userId1);
            //Assert
            Assert.IsTrue(balance1.Money == balance1OpeningMoney + (1 / rate12.Coeff) * moneyToAddInNative);
            Assert.IsTrue(balance12.Money == balance12OpeningMoney + moneyToAddInNative);
            Assert.IsTrue(result == (1 / rate12.Coeff) * moneyToAddInNative);
            balanceRepoMock.Verify(brm => brm.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ThrowArgumentException_WhenBalanceIsNotFound()
        {
            //Arrange
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();

            var currency1 = new Currency { CurrencyName = "USD" };
            var currency2 = new Currency { CurrencyName = "EUR" };
            var balance1 = new Balance { Currency = currency1, UserId = "NotPesho" };
            var balance2 = new Balance { Currency = currency2, UserId = "NotPesho"};
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance> { balance1, balance2 }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.UpdateUserBalanceByAmount(213, "Pesho"));
        }

        [TestMethod]
        public async Task ThrowsArgumentNullException_WhenNativeCurrencyDoesntExists()
        {
            //Arrange
            const string userId = "Pesho";
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();

            var currency1 = new Currency { CurrencyName = "USD" };
            var currency2 = new Currency { CurrencyName = null };
            var balance1 = new Balance { Currency = currency1, UserId = userId, Type = new BalanceType {Name = BalanceTypes.Base.ToString() } };
            var balance2 = new Balance { Currency = currency2, UserId = userId, Type = new BalanceType { Name = BalanceTypes.Personal.ToString() } };
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance> { balance1, balance2 }
                                        .AsQueryable()
                                        .BuildMock()
                                        .Object);

            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var userRepoMock = new Mock<IRepository<User>>();

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.UpdateUserBalanceByAmount(213, "Pesho"));
        }
    }
}
