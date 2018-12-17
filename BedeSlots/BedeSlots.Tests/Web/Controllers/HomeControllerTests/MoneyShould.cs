using BedeSlots.Controllers;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Tests.Web.Controllers.HomeControllerTests
{
    [TestClass]
    public class MoneyShould
    {
        [TestMethod]
        public void ReturnsCorrectViewResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            var transactionServicesMock = new Mock<ITransactionServices>();
            var jsonConverterMock = new Mock<IJsonConverter>();
            var slotGamesServicesMock = new Mock<ISlotGamesServices>();

            var sut = new HomeController(userManagerMock.Object, transactionServicesMock.Object, userServicesMock.Object, slotGamesServicesMock.Object, jsonConverterMock.Object);
            //Act
            var result = sut.Money();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewComponentResult));
        }
    }
}
