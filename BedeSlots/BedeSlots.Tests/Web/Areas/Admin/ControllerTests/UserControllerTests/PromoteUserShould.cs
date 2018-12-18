using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.UserControllerTests
{
    [TestClass]
    public class PromoteUserShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            const string uID = "213123";
            const string role = "Administrator";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userManagerMock
                .Setup(umm => umm.AddToRoleAsync(It.IsAny<User>(), role))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.PromoteUser(uID);
            //Assert
            userManagerMock.Verify(umm => umm.Users, Times.Once);
            userManagerMock.Verify(umm => umm.AddToRoleAsync(It.IsAny<User>(), role), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            const string uID = "213123";
            const string role = "Administrator";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userManagerMock
                .Setup(umm => umm.AddToRoleAsync(It.IsAny<User>(), role))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .SetupGet(umm => umm.Users)
                .Returns(new List<User> { new User { Id = uID } }.AsQueryable());

            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.PromoteUser(uID);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
        [TestMethod]
        public async Task ReturnsPartialViewResultCallingCorrectServices_WhenUserIsNull()
        {
            //Arrange
            const string uID = "213123";
            const string role = "Administrator";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.PromoteUser(uID);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            userManagerMock.Verify(umm => umm.Users, Times.Once);
            userManagerMock.Verify(umm => umm.AddToRoleAsync(It.IsAny<User>(), role), Times.Never);
        }
    }
}
