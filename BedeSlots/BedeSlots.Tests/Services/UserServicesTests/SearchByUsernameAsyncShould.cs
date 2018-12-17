using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.MappingProvider;
using BedeSlots.Services;
using BedeSlots.GlobalData.GlobalViewModels;
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
    public class SearchByUsernameAsyncShould
    {
        [TestMethod]
        public async Task CorrectlyMapAllUsers_WhenUsernameIsNull()
        {
            //Arrange
            var mappingProviderMock = new Mock<IMappingProvider>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();

            var user1 = new User { UserName = "pesho" };
            var user2 = new User { UserName = "gosho" };
            var userList = new List<User> { user1, user2 };
            var userRepoMock = new Mock<IRepository<User>>();
            userRepoMock
                .Setup(urm => urm.All())
                .Returns(userList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act
            var result = await sut.SearchByUsernameAsync(null);
            //Assert
            userRepoMock.Verify(urm => urm.All(), Times.Once);
            mappingProviderMock.Verify(mpp => mpp.MapTo<ICollection<UserViewModel>>(It.IsAny<List<User>>()), Times.Once);
        }
        [TestMethod]
        public async Task CorrectlyMapTheFoundUsers_WhenUsernameIsSpecified()
        {
            //Arrange
            const string searchString = "ru";
            const string nameToBeFound1 = "rumencho";
            const string nameToBeFound2 = "moqtRumencho";
            const string nameNotToBeFound1 = "pesho";
            const string nameNotToBeFound2 = "gosho";

            List<User> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<UserViewModel>>(It.IsAny<List<User>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<User>);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var balanceTypeRepo = new Mock<IRepository<BalanceType>>();
            var userBankDetailsMock = new Mock<IRepository<UserBankDetails>>();

            var userToBeFound1 = new User { UserName = nameToBeFound1 };
            var userToBeFound2 = new User { UserName = nameToBeFound2 };
            var userNotToBeFound1 = new User { UserName = nameNotToBeFound1 };
            var userNotToBeFound2 = new User { UserName = nameNotToBeFound2 };
            var userList = new List<User> { userToBeFound1, userToBeFound2, userNotToBeFound1, userNotToBeFound2 };
            var userRepoMock = new Mock<IRepository<User>>();
            userRepoMock
                .Setup(urm => urm.All())
                .Returns(userList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var sut = new UserServices(userRepoMock.Object, mappingProviderMock.Object, memoryCache, currencyRepoMock.Object, balanceRepoMock.Object, userBankDetailsMock.Object, balanceTypeRepo.Object);
            //Act
            var result = await sut.SearchByUsernameAsync(searchString);
            //Assert
            Assert.IsTrue(mapInput.Count == 2);
            Assert.IsTrue(mapInput.Any(u => u.UserName.ToLower() == nameToBeFound1.ToLower()));
            Assert.IsTrue(mapInput.Any(u => u.UserName.ToLower() == nameToBeFound2.ToLower()));
        }
    }
}
