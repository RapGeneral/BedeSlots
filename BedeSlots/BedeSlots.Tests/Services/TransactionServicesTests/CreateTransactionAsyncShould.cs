using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services;
using BedeSlots.ViewModels.Enums;
using BedeSlots.ViewModels.GlobalViewModels;
using BedeSlots.ViewModels.MappingProvider;
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
    public class CreateTransactionAsyncShould
    {
        [TestMethod]
        public async Task CreateTransaction_WhenInputIsCorrect()
        {
            //Arrange
            const string description = "mn pari";
            const decimal amount = 1232;

            var balance1 = new Balance { Type = new BalanceType { Name = BalanceTypes.Base.ToString() } };
            var balance2 = new Balance { Type = new BalanceType { Name = BalanceTypes.Personal.ToString() } };
            var balanceList = new List<Balance> { balance1, balance2 };

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(br => br.All())
                .Returns(balanceList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var transactionType1 = new TransactionType { Name = "Deposit" };
            var transactionType2 = new TransactionType { Name = "Stake" };
            var typesList = new List<TransactionType> { transactionType1, transactionType2 };

            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();
            transactionTypeRepoMock.Setup(tt => tt.All())
            .Returns(typesList
                .AsQueryable()
                .BuildMock()
                .Object);

            var transactionRepoMock = new Mock<IRepository<Transaction>>();

            Transaction mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock.Setup(mpm => mpm.MapTo<TransactionViewModel>(It.IsAny<Transaction>()))
                .Callback<object>(inputArg => mapInput = inputArg as Transaction);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act
            await sut.CreateTransactionAsync(TypeOfTransaction.Deposit, description, amount, balance1.UserId);

            //Assert
            balanceRepoMock.Verify(br => br.All(), Times.Once);
            transactionTypeRepoMock.Verify(tt => tt.All(), Times.Once);

            Assert.IsTrue(mapInput.Type.Name == TypeOfTransaction.Deposit.ToString());
            Assert.IsTrue(mapInput.Description == description);
            Assert.IsTrue(mapInput.Amount == amount);
            Assert.AreSame(mapInput.Balance, balance1);
            //transaction is created after the balance is changed
            Assert.IsTrue(mapInput.OpeningBalance == balance1.Money - amount);
        }

        [TestMethod]
        public async Task ThrowException_WhenBalanceIsNotFound()
        {
            //Arrange
            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(br => br.All())
                .Returns(new List<Balance>()
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();
            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            var mappingProviderMock = new Mock<IMappingProvider>();

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);

            //Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.CreateTransactionAsync(TypeOfTransaction.Deposit, "pesho", 12, "fdsfsd"));
        }

        [TestMethod]
        public async Task ThrowException_WhenUserIsNotFound()
        {
            //Arrange
            var balance1 = new Balance { Currency = new Currency { CurrencyName = "USD" }, User = new User { Id = "pesho" } };
            var balance2 = new Balance { Currency = new Currency { CurrencyName = "EUR" }, User = new User { Id = "pesho" } };
            var balanceList = new List<Balance> { balance1, balance2 };

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(br => br.All())
                .Returns(balanceList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();
            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            var mappingProviderMock = new Mock<IMappingProvider>();

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);

            //Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.CreateTransactionAsync(TypeOfTransaction.Deposit, "transakciq", 12, "not a pesho"));
        }

        [TestMethod]
        public async Task ThrowException_WhenTransactionTypeIsNotFound()
        {
            //Arrange
            var balance1 = new Balance { Currency = new Currency { CurrencyName = "USD" }, User = new User { Id = "pesho" } };
            var balance2 = new Balance { Currency = new Currency { CurrencyName = "EUR" }, User = new User { Id = "pesho" } };
            var balanceList = new List<Balance> { balance1, balance2 };

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            balanceRepoMock
                .Setup(br => br.All())
                .Returns(balanceList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var transactionType1 = new TransactionType { Name = "Not a Deposit" };
            var transactionType2 = new TransactionType { Name = "Stake" };
            var typesList = new List<TransactionType> { transactionType1, transactionType2 };

            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();
            transactionTypeRepoMock.Setup(tt => tt.All())
            .Returns(typesList
                .AsQueryable()
                .BuildMock()
                .Object);

            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            var mappingProviderMock = new Mock<IMappingProvider>();

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);
            //Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.CreateTransactionAsync(TypeOfTransaction.Deposit, "transakciq", 12, "pesho"));
        }
    }
}