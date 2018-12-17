using BedeSlots.Controllers;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.Providers;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Models;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Tests.Web.Controllers.HomeControllerTests
{
    [TestClass]
    public class SlotGameGetShould
    {
        [TestMethod]
        public void ReturnCorrectViewModel()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var userServicesMock = new Mock<IUserServices>();
            var transactionServicesMock = new Mock<ITransactionServices>();
            var jsonConverterMock = new Mock<IJsonConverter>();
            var slotGamesServicesMock = new Mock<ISlotGamesServices>();

            var sut = new HomeController(userManagerMock.Object, transactionServicesMock.Object, userServicesMock.Object, slotGamesServicesMock.Object, jsonConverterMock.Object);
            //Act
            var result = sut.SlotGame(4, 5) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(SlotGameViewModel));
        }

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
            var result = sut.SlotGame(4, 5); 
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
