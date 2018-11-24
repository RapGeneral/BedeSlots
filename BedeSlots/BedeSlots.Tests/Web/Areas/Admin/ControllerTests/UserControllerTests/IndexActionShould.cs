using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.Areas.Admin.Models;
using BedeSlots.DataModels;
using BedeSlots.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.UserControllerTests
{
    [TestClass]
    public class IndexActionShould
    {
        [TestMethod]
        public void CallCorrectServiceMethod()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = sut.Index(null, null);
            //Assert
            userManagerMock.Verify(s => s.Users, Times.Once);
        }

        [TestMethod]
        public void ReturnCorrectViewModel()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var sut = new UsersController(userManagerMock.Object);
            //Acts
            var result = sut.Index(null, null) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(IndexViewModel));
        }

        [TestMethod]
        public void ReturnsCorrectViewResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var sut = new UsersController(userManagerMock.Object);
            //Act
            var result = sut.Index(null, null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
