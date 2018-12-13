using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.MappingProvider;
using BedeSlots.Services;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Services.TransactionServicesTests
{
    [TestClass]
    public class SearchTransactionAsyncShould
    {
        [TestMethod]
        public async Task CorrectlySearch_WhenUsernameIsNull()
        {
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();

            var balance1 = new Balance { User = new User { UserName = "pesho" } };
            var balance2 = new Balance { User = new User { UserName = "" } };

            List<string> types = new List<string>();

            var transaction1 = new Transaction { Balance = balance1 };
            var transaction2 = new Transaction { Balance = balance2 };
            var transactionsList = new List<Transaction> { transaction1, transaction2 };

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            transactionRepoMock.Setup(trp => trp.All())
                .Returns(transactionsList
                    .AsQueryable()
                    .BuildMock()
                    .Object);
            
            List<Transaction> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<Transaction>);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            var result = await sut.SearchTransactionAsync(null, null, null, types);
            //Assert
            transactionRepoMock.Verify(urm => urm.All(), Times.Once);
            mappingProviderMock.Verify(mpp => mpp.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()), Times.Once);
            Assert.IsTrue(mapInput.Count == 2);
        }
        
        [TestMethod]
        public async Task CorrectlySearch_WhenUsernameIsSpecified()
        {
            const string searchUsername = "pe";

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();

            var balance1 = new Balance { User = new User { UserName = "pesho" } };
            var balance2 = new Balance { User = new User { UserName = "" } };
            var balance3 = new Balance { User = new User { UserName = "pavkata" } };

            List<string> types = new List<string>();

            var transaction1 = new Transaction { Balance = balance1 };
            var transaction2 = new Transaction { Balance = balance2 };
            var transaction3 = new Transaction { Balance = balance3 };
            var transactionsList = new List<Transaction> { transaction1, transaction2, transaction3 };

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            transactionRepoMock.Setup(trp => trp.All())
                .Returns(transactionsList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            List<Transaction> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<Transaction>);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            var result = await sut.SearchTransactionAsync(searchUsername, null, null, types);
            //Assert
            transactionRepoMock.Verify(urm => urm.All(), Times.Once);
            mappingProviderMock.Verify(mpp => mpp.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()), Times.Once);
            Assert.IsTrue(mapInput.Count == 1);
        }

        [TestMethod]
        public async Task CorrectlySearch_WhenMinAndMaxAmountIsNull()
        {
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();           

            List<string> types = new List<string>();

            var transaction1 = new Transaction { Amount = 123 };
            var transaction2 = new Transaction { Amount = 0 };
            var transaction3 = new Transaction { Amount = (decimal)343.23 };

            var transactionsList = new List<Transaction> { transaction1, transaction2, transaction3 };

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            transactionRepoMock.Setup(trp => trp.All())
                .Returns(transactionsList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            List<Transaction> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<Transaction>);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            var result = await sut.SearchTransactionAsync(null, null, null, types);
            //Assert
            transactionRepoMock.Verify(urm => urm.All(), Times.Once);
            mappingProviderMock.Verify(mpp => mpp.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()), Times.Once);
            Assert.IsTrue(mapInput.Count == 3);
        }

        [TestMethod]
        public async Task CorrectlySearch_WhenMaxIsSpecified()
        {
            const int maxSearch = 23;

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();

            List<string> types = new List<string>();

            var transaction1 = new Transaction { Amount = 22 };
            var transaction2 = new Transaction { Amount = 14};
            var transaction3 = new Transaction { Amount = (decimal)343.23 };

            var transactionsList = new List<Transaction> { transaction1, transaction2, transaction3 };

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            transactionRepoMock.Setup(trp => trp.All())
                .Returns(transactionsList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            List<Transaction> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<Transaction>);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            var result = await sut.SearchTransactionAsync(null, null, maxSearch, types);
            //Assert
            transactionRepoMock.Verify(urm => urm.All(), Times.Once);
            mappingProviderMock.Verify(mpp => mpp.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()), Times.Once);
            Assert.IsTrue(mapInput.Count == 2);
            Assert.IsTrue(mapInput.Any(tr => tr.Balance == transaction1.Balance));
            Assert.IsTrue(mapInput.Any(tr => tr.Balance == transaction2.Balance));
        }

        [TestMethod]
        public async Task CorrectlySearch_WhenMinIsSpecified()
        {
            const int minSearch = 23;

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();

            List<string> types = new List<string>();

            var transaction1 = new Transaction { Amount = 22 };
            var transaction2 = new Transaction { Amount = 14 };
            var transaction3 = new Transaction { Amount = (decimal)343.23 };

            var transactionsList = new List<Transaction> { transaction1, transaction2, transaction3 };

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            transactionRepoMock.Setup(trp => trp.All())
                .Returns(transactionsList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            List<Transaction> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<Transaction>);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            var result = await sut.SearchTransactionAsync(null, minSearch, null, types);
            //Assert
            transactionRepoMock.Verify(urm => urm.All(), Times.Once);
            mappingProviderMock.Verify(mpp => mpp.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()), Times.Once);
            Assert.IsTrue(mapInput.Count == 1);
            Assert.IsTrue(mapInput.Any(tr => tr.Balance == transaction3.Balance));
        }

        [TestMethod]
        public async Task ReturnsEmptyList_WhenMinIsBiggerThanMax()
        {
            const int minSearch = 23;
            const int maxSearch = 10;

            var mappingProviderMock = new Mock<IMappingProvider>();
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();

            List<string> types = new List<string>();

            var transaction1 = new Transaction { Amount = 22 };
            var transaction2 = new Transaction { Amount = 14 };
            var transaction3 = new Transaction { Amount = (decimal)343.23 };

            var transactionsList = new List<Transaction> { transaction1, transaction2, transaction3 };

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            transactionRepoMock.Setup(trp => trp.All())
                .Returns(transactionsList
                    .AsQueryable()
                    .BuildMock()
                    .Object);
            
            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            var result = await sut.SearchTransactionAsync(null, minSearch, maxSearch, types);
            //Assert
            transactionRepoMock.Verify(urm => urm.All(), Times.Once);
            
            Assert.IsTrue(result.Count == 0);
        }
    }
}