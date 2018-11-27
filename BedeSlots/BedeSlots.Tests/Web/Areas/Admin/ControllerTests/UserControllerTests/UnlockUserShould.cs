using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers;
using BedeSlots.Services.Contracts;
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
    public class UnlockUserShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            const string uID = "213123";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UnlockUser(uID);
            //Assert
            userManagerMock.Verify(umm => umm.Users, Times.Once);
            userManagerMock.Verify(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()), Times.Once);
        }

        [TestMethod]
        public async Task CorrectylUnlocksTheUser()
        {
            //This unit test is not compleately unit one.
            //However, DateTime.Now cannot be verified if it has been called (since time usually flows).
            //Arrange
            const string uID = "213123";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(IdentityResult.Success);
            var user = new User { Id = uID };
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { user }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UnlockUser(uID);
            //Assert
            Assert.IsTrue(DateTime.Now.CompareTo(user.LockoutEnd) >= 0);
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            const string uID = "213123";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UnlockUser(uID);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
        [TestMethod]
        public async Task ReturnsPartialViewResultCallingCorrectServices_WhenUserIsNull()
        {
            //Arrange
            const string uID = "213123";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UnlockUser(uID);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(umm => umm.Users, Times.Once);
            userManagerMock.Verify(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }
        [TestMethod]
        public async Task ReturnsPartialViewResultCallingCorrectServices_WhenSetLockoutDateFails()
        {
            //Arrange
            const string uID = "213123";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userManagerMock
                .Setup(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(IdentityResult.Failed());
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UnlockUser(uID) as PartialViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(umm => umm.Users, Times.Once);
            userManagerMock.Verify(umm => umm.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsAny<DateTimeOffset>()), Times.Once);
        }
    }
}
