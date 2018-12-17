using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.Providers;
using BedeSlots.Services.Contracts;
using BedeSlots.GlobalData.GlobalViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.UserControllerTests
{
    [TestClass]
    public class UserGridShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            const string userName = "somename";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.SearchByUsernameAsync(userName))
                .ReturnsAsync(new List<UserViewModel>());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UserGrid(21, userName) as PartialViewResult;
            //Assert
            userServicesMock.Verify(usm => usm.SearchByUsernameAsync(userName), Times.Once);
        }

        [TestMethod]
        public async Task ReturnCorrectViewModel()
        {
            //Arrange
            const string userName = "somename";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.SearchByUsernameAsync(userName))
                .ReturnsAsync(new List<UserViewModel>());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Acts
            var result = await sut.UserGrid(21, userName) as PartialViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(IPagedList<UserViewModel>));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            const string userName = "somename";
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.SearchByUsernameAsync(userName))
                .ReturnsAsync(new List<UserViewModel>());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.UserGrid(21, userName);
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
    }
}
