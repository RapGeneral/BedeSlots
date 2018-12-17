using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.TransactionsControllerTests
{
    [TestClass]
    public class TransactionGridShould
    {
        [TestMethod]
        public async Task CallCorrectServiceMethod()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var transactionServicesMock = new Mock<ITransactionServices>();
            transactionServicesMock
                .Setup(tsm => tsm.SearchTransactionAsync(null, null, null, null, null, false))
                .ReturnsAsync(new List<TransactionViewModel>());

            var sut = new TransactionsController(userManagerMock.Object, transactionServicesMock.Object, memoryCache);
            //Act
            var result = await sut.TransactionGrid(null, null, null, null, null, null, false);
            //Assert
            transactionServicesMock.Verify(tsm => tsm.SearchTransactionAsync(null, null, null, null, null, false), Times.Once);
        }

        [TestMethod]
        public async Task ReturnCorrectViewModel()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var transactionServicesMock = new Mock<ITransactionServices>();
            transactionServicesMock
                .Setup(tsm => tsm.SearchTransactionAsync(null, null, null, null, null, false))
                .ReturnsAsync(new List<TransactionViewModel>());

            var sut = new TransactionsController(userManagerMock.Object, transactionServicesMock.Object, memoryCache);
            //Acts
            var result = await sut.TransactionGrid(null, null, null, null, null, null, false) as PartialViewResult;
            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(IPagedList<TransactionViewModel>));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserManager<User>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var transactionServicesMock = new Mock<ITransactionServices>();
            transactionServicesMock
                .Setup(tsm => tsm.SearchTransactionAsync(null, null, null, null, null, false))
                .ReturnsAsync(new List<TransactionViewModel>());

            var sut = new TransactionsController(userManagerMock.Object, transactionServicesMock.Object, memoryCache);
            //Act
            var result = await sut.TransactionGrid(null, null, null, null, null, null, false) as PartialViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
    }
}
