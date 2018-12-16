using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.DataModels;
using BedeSlots.ViewModels.Providers;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.UserControllerTests
{
    [TestClass]
    public class IndexActionShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.SearchByUsernameAsync(null))
                .ReturnsAsync(new List<UserViewModel>());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.Index(null, null);
            //Assert
            userServicesMock.Verify(usm => usm.SearchByUsernameAsync(null), Times.Once);
        }

        [TestMethod]
        public async Task ReturnCorrectViewModel()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.SearchByUsernameAsync(null))
                .ReturnsAsync(new List<UserViewModel>());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Acts
            var result = await sut.Index(null, null) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(IPagedList<UserViewModel>));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.SearchByUsernameAsync(null))
                .ReturnsAsync(new List<UserViewModel>());
            var sut = new UsersController(userManagerMock.Object, userServicesMock.Object);
            //Act
            var result = await sut.Index(null, null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
