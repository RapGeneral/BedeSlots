using BedeSlots.Areas.Identity.Controllers;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Areas.Identity.Controllers.AccountControllerTests
{
    [TestClass]
    public class LogoutShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            //Act
            var result = await sut.Logout();
            //Assert
            signInManagerMock.Verify(simm => simm.SignOutAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            //Act
            var result = await sut.Logout();
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
