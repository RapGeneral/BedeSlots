using BedeSlots.Areas.Identity.Controllers;
using BedeSlots.Areas.Identity.Models.AccountViewModels;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.Enums;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Areas.Identity.Controllers.AccountControllerTests
{
    [TestClass]
    public class RegisterPostShould
    {
        [TestMethod]
        public async Task CallCorrectServicesWhenRegisterModelIsValidAndRegisterSuccessfull()
        {
            //Arrange
            const string password = "zaq1@WSX";
            const string currencyName = "EUR";
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Act
            var result = await sut.Register(new RegisterViewModel { Password = password, CurrencyName = currencyName});
            //Assert
            userManagerMock.Verify(umm => umm.CreateAsync(It.IsAny<User>(), password), Times.Once);
            userServicesMock.Verify(usm => usm.CreateUserInitialBalances(It.IsAny<string>(), currencyName), Times.Once);
            userManagerMock.Verify(umm => umm.AddToRoleAsync(It.IsAny<User>(), UserRoles.User.ToString()));
            signInManagerMock.Verify(simm => simm.SignInAsync(It.IsAny<User>(), false));
        }
        [TestMethod]
        public async Task ReturnsCorrectViewResult_WhenRegisterModelIsValidAndRegisterSuccessfull()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Act
            var result = await sut.Register(new RegisterViewModel());
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
        [TestMethod]
        public async Task ReturnsCorrectViewResult_WhenRegisterModelIsValidAndRegisternsuccessfull()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Act
            var result = await sut.Register(new RegisterViewModel());
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public async Task ReturnCorrectViewModel_WhenRegisterModelIsValidAndRegisterUsuccessfull()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);

            //Act
            var result = await sut.Register(new RegisterViewModel()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(RegisterViewModel));
        }
        [TestMethod]
        public async Task ReturnsCorrectViewResult_WhenRegisterModelIsInvalid()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            sut.ModelState.AddModelError("", "");
            //Act
            var result = await sut.Register(new RegisterViewModel());
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnCorrectViewModel_WhenRegisterModelIsInvalid()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            sut.ModelState.AddModelError("", "");
            //Act
            var result = await sut.Register(new RegisterViewModel()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(RegisterViewModel));
        }
    }
}
