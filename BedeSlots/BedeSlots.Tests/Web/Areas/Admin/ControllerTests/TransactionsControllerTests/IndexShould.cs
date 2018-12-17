using BedeSlots.Areas.Admin.Controllers;
using BedeSlots.Areas.Admin.Models;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.GlobalViewModels;
using BedeSlots.Infrastructure.Providers.Interfaces;
using BedeSlots.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Web.Areas.Admin.ControllerTests.TransactionsControllerTests
{
    [TestClass]
    public class IndexShould
    {
        [TestClass]
        public class IndexActionShould
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
                transactionServicesMock
                    .Setup(tsm => tsm.GetTypesAsync())
                    .ReturnsAsync(new List<string>());

                var sut = new TransactionsController(userManagerMock.Object, transactionServicesMock.Object, memoryCache);
                //Act
                var result = await sut.Index(null, null, null, null, null, null, false);
                var result2 = await sut.Index(null, null, null, null, null, null, false);
                //Assert
                transactionServicesMock.Verify(tsm => tsm.SearchTransactionAsync(null, null, null, null, null, false), Times.Exactly(2));
                transactionServicesMock.Verify(tsm => tsm.GetTypesAsync(), Times.Once);
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
                transactionServicesMock
                    .Setup(tsm => tsm.GetTypesAsync())
                    .ReturnsAsync(new List<string>());

                var sut = new TransactionsController(userManagerMock.Object, transactionServicesMock.Object, memoryCache);
                //Acts
                var result = await sut.Index(null, null, null, null, null, null, false) as ViewResult;
                //Assert
                Assert.IsInstanceOfType(result.Model, typeof(IndexViewModel));
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
                transactionServicesMock
                    .Setup(tsm => tsm.GetTypesAsync())
                    .ReturnsAsync(new List<string>());

                var sut = new TransactionsController(userManagerMock.Object, transactionServicesMock.Object, memoryCache);
                //Act
                var result = await sut.Index(null, null, null, null, null, null, false);
                //Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }
        }
    }
}
