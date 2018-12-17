using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.GlobalData.MappingProvider;
using BedeSlots.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Services.UserBankDetailsTests
{
    [TestClass]
    public class DeleteUserBankDetailsShould
    {
        [DataTestMethod]
        [DataRow("random", null)]
        [DataRow(null, "random")]
        public async Task ThrowArgumentNullException_WhenArgumentsAreNull(string userId, string bankDetailsId)
        {
            //Arrange
            var mappingProviderMock = new Mock<IMappingProvider>();

            var userBankDetailsRepoMock = new Mock<IRepository<UserBankDetails>>();

            var sut = new UserBankDetailsServices(mappingProviderMock.Object, userBankDetailsRepoMock.Object);
            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.DeleteUserBankDetailsAsync(bankDetailsId, userId));
        }
        [TestMethod]
        public async Task ReturnTheBankDetils_WhenTheyAreDeleted()
        {
            //Arrange
            const string user1Id = "poesho";
            const string user2Id = "goesho";
            Guid bankDetails1Id = Guid.NewGuid();
            Guid bankDetails2Id = Guid.NewGuid();

            BankDetails mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<BankDetailsViewModel>(It.IsAny<BankDetails>()))
                .Callback<object>(inputArg => mapInput = inputArg as BankDetails);

            var user1 = new User { Id = user1Id};
            var user2 = new User { Id = user2Id };
            var bankDetails1 = new BankDetails { Id = bankDetails1Id };
            var bankDetails2 = new BankDetails { Id = bankDetails2Id };
            var userBankDetails11 = new UserBankDetails {UserId = user1Id, BankDetailsId = bankDetails1Id, BankDetails = bankDetails1 };
            var userBankDetails12 = new UserBankDetails {UserId = user1Id, BankDetailsId = bankDetails2Id, BankDetails = bankDetails2 };
            var userBankDetails21 = new UserBankDetails {UserId = user2Id, BankDetailsId = bankDetails1Id, BankDetails = bankDetails1 };
            var userBankDetails22 = new UserBankDetails {UserId = user2Id, BankDetailsId = bankDetails2Id, BankDetails = bankDetails2 };
            var userBankDetailsRepoMock = new Mock<IRepository<UserBankDetails>>();
            userBankDetailsRepoMock
                .Setup(ubdrm => ubdrm.All())
                .Returns(new List<UserBankDetails> { userBankDetails11, userBankDetails12, userBankDetails21, userBankDetails22 }
                                    .AsQueryable()
                                    .BuildMock()
                                    .Object);

            var sut = new UserBankDetailsServices(mappingProviderMock.Object, userBankDetailsRepoMock.Object);
            //Act
            var result = await sut.DeleteUserBankDetailsAsync(bankDetails2Id.ToString(),user1Id);
            //Assert
            Assert.IsTrue(userBankDetails12.IsDeleted);
            mappingProviderMock.Verify(mpm => mpm.MapTo<BankDetailsViewModel>(bankDetails2), Times.Once);
            userBankDetailsRepoMock.Verify(ubdr => ubdr.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ThrowArgumentException_WhenUserBankDetailsAreNotFoundByUser()
        {
            //Arrange
            const string user1Id = "poesho";
            const string user2Id = "goesho";
            Guid bankDetails1Id = Guid.NewGuid();
            Guid bankDetails2Id = Guid.NewGuid();

            BankDetails mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<BankDetailsViewModel>(It.IsAny<BankDetails>()))
                .Callback<object>(inputArg => mapInput = inputArg as BankDetails);

            var user1 = new User { Id = user1Id };
            var user2 = new User { Id = user2Id };
            var bankDetails1 = new BankDetails { Id = bankDetails1Id };
            var bankDetails2 = new BankDetails { Id = bankDetails2Id };
            var userBankDetails11 = new UserBankDetails { UserId = user1Id, BankDetailsId = bankDetails1Id, BankDetails = bankDetails1 };
            var userBankDetails12 = new UserBankDetails { UserId = user1Id, BankDetailsId = bankDetails2Id, BankDetails = bankDetails2 };
            var userBankDetails21 = new UserBankDetails { UserId = user2Id, BankDetailsId = bankDetails1Id, BankDetails = bankDetails1 };
            var userBankDetails22 = new UserBankDetails { UserId = user2Id, BankDetailsId = bankDetails2Id, BankDetails = bankDetails2 };
            var userBankDetailsRepoMock = new Mock<IRepository<UserBankDetails>>();
            userBankDetailsRepoMock
                .Setup(ubdrm => ubdrm.All())
                .Returns(new List<UserBankDetails> { userBankDetails11, userBankDetails12, userBankDetails21, userBankDetails22 }
                                    .AsQueryable()
                                    .BuildMock()
                                    .Object);

            var sut = new UserBankDetailsServices(mappingProviderMock.Object, userBankDetailsRepoMock.Object);
            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.DeleteUserBankDetailsAsync(bankDetails2Id.ToString(), "not" + user2Id));
        }

        [TestMethod]
        public async Task ThrowArgumentException_WhenUserBankDetailsAreNotFoundById()
        {
            //Arrange
            const string user1Id = "poesho";
            const string user2Id = "goesho";
            Guid bankDetails1Id = Guid.NewGuid();
            Guid bankDetails2Id = Guid.NewGuid();

            BankDetails mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<BankDetailsViewModel>(It.IsAny<BankDetails>()))
                .Callback<object>(inputArg => mapInput = inputArg as BankDetails);

            var user1 = new User { Id = user1Id };
            var user2 = new User { Id = user2Id };
            var bankDetails1 = new BankDetails { Id = bankDetails1Id };
            var bankDetails2 = new BankDetails { Id = bankDetails2Id };
            var userBankDetails11 = new UserBankDetails { UserId = user1Id, BankDetailsId = bankDetails1Id, BankDetails = bankDetails1 };
            var userBankDetails12 = new UserBankDetails { UserId = user1Id, BankDetailsId = bankDetails2Id, BankDetails = bankDetails2 };
            var userBankDetails21 = new UserBankDetails { UserId = user2Id, BankDetailsId = bankDetails1Id, BankDetails = bankDetails1 };
            var userBankDetails22 = new UserBankDetails { UserId = user2Id, BankDetailsId = bankDetails2Id, BankDetails = bankDetails2 };
            var userBankDetailsRepoMock = new Mock<IRepository<UserBankDetails>>();
            userBankDetailsRepoMock
                .Setup(ubdrm => ubdrm.All())
                .Returns(new List<UserBankDetails> { userBankDetails11, userBankDetails12, userBankDetails21, userBankDetails22 }
                                    .AsQueryable()
                                    .BuildMock()
                                    .Object);

            var sut = new UserBankDetailsServices(mappingProviderMock.Object, userBankDetailsRepoMock.Object);
            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.DeleteUserBankDetailsAsync("not" + bankDetails2Id.ToString(), user2Id));
        }
    }
}
