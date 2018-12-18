using BedeSlots.Areas.Identity.Controllers;
using BedeSlots.Areas.Identity.Models.AccountViewModels;
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
    public class LoginPostShould
    {
        [TestMethod]
        public async Task ReturnsCorrectViewResult_WhenLoginModelIsValidAndLoginSuccessfull()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();

            var signInManagerMock = new Mock<ISignInManager<User>>();
            signInManagerMock
                .Setup(simm => simm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Act
            var result = await sut.Login(new LoginViewModel());
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
        [TestMethod]
        public async Task ReturnsCorrectViewResult_WhenLoginModelIsValidAndLoginUnsuccessfull()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();

            var signInManagerMock = new Mock<ISignInManager<User>>();
            signInManagerMock
                .Setup(simm => simm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Act
            var result = await sut.Login(new LoginViewModel());
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public async Task ReturnCorrectViewModel_WhenLoginModelIsValidAndLoginUnsuccessfull()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();

            var signInManagerMock = new Mock<ISignInManager<User>>();
            signInManagerMock
                .Setup(simm => simm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Acts
            var result = await sut.Login(new LoginViewModel()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(LoginViewModel));
        }
        [TestMethod]
        public async Task ReturnsCorrectViewResult_WhenLoginModelIsInvalid()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            sut.ModelState.AddModelError("", "");
            //Act
            var result = await sut.Login(new LoginViewModel());
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnCorrectViewModel_WhenLoginModelIsInvalid()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            sut.ModelState.AddModelError("", "");
            //Act
            var result = await sut.Login(new LoginViewModel()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(LoginViewModel));
        }
    }
}
