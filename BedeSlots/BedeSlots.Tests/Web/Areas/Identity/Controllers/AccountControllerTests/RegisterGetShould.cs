using BedeSlots.Areas.Identity.Controllers;
using BedeSlots.Areas.Identity.Models.AccountViewModels;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Areas.Identity.Controllers.AccountControllerTests
{
    [TestClass]
    public class RegisterGetShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();

            Mock<ISignInManager<User>> signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            //Act
            var result = await sut.Register();
            var result2 = await sut.Register();
            //Assert
            currencyServicesMock.Verify(crm => crm.GetCurrenciesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ReturnCorrectViewModel()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();

            Mock<ISignInManager<User>> signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            //Act
            var result = await sut.Register() as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(RegisterViewModel));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();

            Mock<ISignInManager<User>> signInManagerMock = new Mock<ISignInManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var userServicesMock = new Mock<IUserServices>();
            var currencyServicesMock = new Mock<ICurrencyServices>();
            currencyServicesMock
                .Setup(csm => csm.GetCurrenciesAsync())
                .ReturnsAsync(new List<string>());

            var sut = new AccountController(userManagerMock.Object, signInManagerMock.Object, memoryCache, userServicesMock.Object, currencyServicesMock.Object);
            //Act
            var result = await sut.Register();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}