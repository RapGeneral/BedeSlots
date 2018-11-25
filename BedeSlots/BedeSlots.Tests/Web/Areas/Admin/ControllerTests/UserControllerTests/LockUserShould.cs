using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.DataModels;
using BedeSlots.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.UserControllerTests
{
    [TestClass]
    public class LockUserShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethods()
        {
            //Arrange
            const string uID = "213123";
            const int validTimePeriod = 123;
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEnabledAsync(It.IsAny<User>(), true))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), DateTime.Today.AddDays(validTimePeriod)))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = await sut.LockUser(uID, validTimePeriod);
            //Assert
            userManagerMock.Verify(s => s.Users, Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEnabledAsync(It.IsAny<User>(), true), Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEndDateAsync(It.IsAny<User>(), DateTime.Today.AddDays(validTimePeriod)), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            const string uID = "213123";
            const int validTimePeriod = 123;
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEnabledAsync(It.IsAny<User>(), true))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = await sut.LockUser(uID, validTimePeriod);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(36001)]
        public async Task ReturnsPartialViewResultCallingCorrectServices_DurationIsInvalid(int validTimePeriod)
        {
            //Arrange
            const string uID = "213123";
            var userManagerMock = new Mock<IUserManager<User>>();
            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = await sut.LockUser(uID, validTimePeriod);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(s => s.Users, Times.Never);
            userManagerMock.Verify(s => s.SetLockoutEnabledAsync(It.IsAny<User>(), It.IsAny<bool>()), Times.Never);
            userManagerMock.Verify(s => s.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }
        [TestMethod]
        public async Task ReturnsPartialViewResultCallingCorrectServices_WhenUserIsNull()
        {
            //Arrange
            const string uID = "213123";
            const int validTimePeriod = 123;
            var userManagerMock = new Mock<IUserManager<User>>();
            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = await sut.LockUser(uID, validTimePeriod);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(s => s.Users, Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEnabledAsync(It.IsAny<User>(), It.IsAny<bool>()), Times.Never);
            userManagerMock.Verify(s => s.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }
        [TestMethod]
        public async Task ReturnsPartialViewResultCallingCorrectServices_WhenSetLockoutDateFails()
        {
            //Arrange
            const string uID = "213123";
            const int validTimePeriod = 123;
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEnabledAsync(It.IsAny<User>(), true))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(IdentityResult.Failed());
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = await sut.LockUser(uID, validTimePeriod);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(s => s.Users, Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEnabledAsync(It.IsAny<User>(), true), Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEndDateAsync(It.IsAny<User>(), DateTime.Today.AddDays(validTimePeriod)), Times.Once);
        }
        [TestMethod]
        public async Task ReturnsPartialViewResultCallingCorrectServices_WhenSetLockoutFails()
        {
            //Arrange
            const string uID = "213123";
            const int validTimePeriod = 123;
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEnabledAsync(It.IsAny<User>(), true))
                .ReturnsAsync(IdentityResult.Failed());
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());
            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = await sut.LockUser(uID, validTimePeriod);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(s => s.Users, Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEnabledAsync(It.IsAny<User>(), true), Times.Once);
            userManagerMock.Verify(s => s.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }
    }
}
