using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.MappingProvider;
using BedeSlots.Services;
using BedeSlots.ViewModels.Enums;
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
    public class CreateTransactionAsyncShould
    {
        [TestMethod]
        public async Task CreateTransaction_WhenInputIsCorrect()
        {
            const string description = "mn pari";
            const decimal amount = 1232;

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

            List<Transaction> mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock.Setup(mpm => mpm.MapTo<ICollection<TransactionViewModel>>(It.IsAny<List<Transaction>>()))
                .Callback<object>(inputArg => mapInput = inputArg as List<Transaction>);

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);            

            var result = await sut.CreateTransactionAsync(TypeOfTransaction.Deposit, description, amount, balance1.UserId);

            balanceRepoMock.Verify(br => br.All(), Times.Once);
            transactionTypeRepoMock.Verify(tt => tt.All(), Times.Once);
        }
    }
}