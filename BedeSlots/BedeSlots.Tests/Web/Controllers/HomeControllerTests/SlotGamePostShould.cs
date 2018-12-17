using BedeSlots.Controllers;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.Enums;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.Infrastructure.Providers;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Models;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Controllers.HomeControllerTests
{
    [TestClass]
    public class SlotGamePostShould
    {
        [DataTestMethod]
        [DataRow(0.0, 1, 1)]
        [DataRow(1235.0, 2, 2)]
        public async Task CallCorrectServiceMethod(double coeff, int createTransactionCallTimes, int updateBalanceCallTimes)
        {
            //Arrange
            const int stakeAmountNative = 20;
            const int stakeInUSD = 10;
            const int balAmount = 50;
            const int M = 4;
            const int N = 5;
            const string uId = "dawda";
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(uId);

            var balanceInfo = new MoneyViewModel { Amount = balAmount };
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.GetBalanceInformation(uId))
                .ReturnsAsync(balanceInfo);
            userServicesMock
                .Setup(usm => usm.UpdateUserBalanceByAmount(It.IsAny<decimal>(), uId))
                .ReturnsAsync(stakeInUSD);

            var transactionServicesMock = new Mock<ITransactionServices>();

            var slotMatix = new List<List<GameItemChanceOutOf100>>();
            var slotGamesServicesMock = new Mock<ISlotGamesServices>();
            slotGamesServicesMock
                .Setup(sgsm => sgsm.Run(N, M))
                .Returns(slotMatix);
            slotGamesServicesMock
                .Setup(sgsm => sgsm.Evaluate(slotMatix))
                .Returns((decimal)coeff);

            var jsonConverterMock = new Mock<IJsonConverter>();

            var sut = new HomeController(userManagerMock.Object, transactionServicesMock.Object, userServicesMock.Object, slotGamesServicesMock.Object, jsonConverterMock.Object);
            //Act
            var result = await sut.SlotGame(new SlotGameViewModel { M = M, N = N, Stake = stakeAmountNative });
            //Assert
            userServicesMock.Verify(usm => usm.GetBalanceInformation(uId), Times.Once);
            userServicesMock.Verify(usm => usm.UpdateUserBalanceByAmount(It.IsAny<decimal>(), uId), Times.Exactly(updateBalanceCallTimes));
            transactionServicesMock.Verify(tsm => tsm.CreateTransactionAsync(It.IsAny<TypeOfTransaction>(), It.IsAny<string>(), It.IsAny<decimal>(), uId), Times.Exactly(createTransactionCallTimes));
            slotGamesServicesMock.Verify(sgsm => sgsm.Run(N, M), Times.Once);
            slotGamesServicesMock.Verify(sgsm => sgsm.Evaluate(slotMatix), Times.Once);
            jsonConverterMock.Verify(jsm => jsm.SerializeObject(slotMatix, It.IsAny<JsonSerializerSettings>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            const int stakeAmountNative = 20;
            const int stakeInUSD = 10;
            const int balAmount = 50;
            const int M = 4;
            const int N = 5;
            const decimal coeff = 0;
            const string uId = "dawda";
            var userManagerMock = new Mock<IUserManager<User>>();
            userManagerMock
                .Setup(umm => umm.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(uId);

            var balanceInfo = new MoneyViewModel { Amount = balAmount };
            var userServicesMock = new Mock<IUserServices>();
            userServicesMock
                .Setup(usm => usm.GetBalanceInformation(uId))
                .ReturnsAsync(balanceInfo);
            userServicesMock
                .Setup(usm => usm.UpdateUserBalanceByAmount(It.IsAny<decimal>(), uId))
                .ReturnsAsync(stakeInUSD);

            var transactionServicesMock = new Mock<ITransactionServices>();

            var slotMatix = new List<List<GameItemChanceOutOf100>>();
            var slotGamesServicesMock = new Mock<ISlotGamesServices>();
            slotGamesServicesMock
                .Setup(sgsm => sgsm.Run(N, M))
                .Returns(slotMatix);
            slotGamesServicesMock
                .Setup(sgsm => sgsm.Evaluate(slotMatix))
                .Returns(coeff);

            var jsonConverterMock = new Mock<IJsonConverter>();

            var sut = new HomeController(userManagerMock.Object, transactionServicesMock.Object, userServicesMock.Object, slotGamesServicesMock.Object, jsonConverterMock.Object);
            //Act
            var result = await sut.SlotGame(new SlotGameViewModel { M = M, N = N, Stake = stakeAmountNative });
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
    }
}
