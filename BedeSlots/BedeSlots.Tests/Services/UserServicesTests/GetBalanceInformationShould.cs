using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services;
using BedeSlots.ViewModels.Enums;
using BedeSlots.ViewModels.GlobalViewModels;
using BedeSlots.ViewModels.MappingProvider;
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
    public class GetBalanceInformationShould
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
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetBalanceInformation(null));
        }
        [TestMethod]
        public async Task ReturnCorrectBalance()
        {
            //Arrange
            const string userId = "213124";

            var mapInput = new Balance();
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<MoneyViewModel>(It.IsAny<Balance>()))
                .Callback<object>(inputArg => mapInput = inputArg as Balance);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();

            var balance1 = new Balance { Type = new BalanceType { Name = BalanceTypes.Base.ToString() }, UserId = userId };
            var balance12 = new Balance { Type = new BalanceType { Name = BalanceTypes.Personal.ToString() }, UserId = userId };
            var balance2 = new Balance { Type = new BalanceType { Name = BalanceTypes.Base.ToString() }, UserId = "not" + userId };
            var balance22 = new Balance { Type = new BalanceType { Name = BalanceTypes.Personal.ToString() }, UserId = "not" + userId };
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance> {balance1, balance12, balance2, balance22 }
                                .AsQueryable()
                                .BuildMock()
                                .Object);

            var userRepoMock = new Mock<IRepository<User>>();
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act
            var result = await sut.GetBalanceInformation(userId);
            //Assert
            Assert.AreSame(balance12, mapInput);
        }
        [TestMethod]
        public async Task ThrowArgumentException_BalanceIsNotFound()
        {
            //Arrange
            const string userId = "213124";
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();

            var balance1 = new Balance { Type = new BalanceType { Name = BalanceTypes.Base.ToString() }, UserId = "not" + userId };
            var balance2 = new Balance { Type = new BalanceType { Name = BalanceTypes.Personal.ToString() }, UserId = "not" + userId };
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(brm => brm.All())
                .Returns(new List<Balance> { balance1, balance2 }
                                .AsQueryable()
                                .BuildMock()
                                .Object);

            var userRepoMock = new Mock<IRepository<User>>();
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();
            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object);
            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.GetBalanceInformation(userId));
        }
    }
}
