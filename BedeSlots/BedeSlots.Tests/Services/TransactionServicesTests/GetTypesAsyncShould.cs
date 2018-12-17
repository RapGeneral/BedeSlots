using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services;
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
    public class GetTypesAsyncShould
    {
        [TestMethod]
        public async Task ShouldReturnAll_WhenInvoked()
        {
            List<TransactionType> transactionTypesList = new List<TransactionType>();
            var transactionTypeRepoMock = new Mock<IRepository<TransactionType>>();
            transactionTypeRepoMock.Setup(trp => trp.All())
                .Returns(transactionTypesList
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var balanceRepoMock = new Mock<IRepository<Balance>>();
            var transactionRepoMock = new Mock<IRepository<Transaction>>();
            var mappingProviderMock = new Mock<IMappingProvider>();

            var sut = new TransactionServices(balanceRepoMock.Object, transactionRepoMock.Object, transactionTypeRepoMock.Object, mappingProviderMock.Object);

            var result = await sut.GetTypesAsync();

            Assert.IsNotNull(result);
        }
    }
}
