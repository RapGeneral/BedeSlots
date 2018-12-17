using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Services.CurrencyServicesTests
{
    [TestClass]
    public class GetCurrenciesAsyncShould
    {
        [TestMethod]
        public async Task ReturnAllCurrencies()
        {
            //Arrange
            var currency1 = new Currency { CurrencyName = "USD" };
            var currency2 = new Currency { CurrencyName = "BGN" };
            var currency3 = new Currency { CurrencyName = "EUR" };
            var currencyRepoMock = new Mock<IRepository<Currency>>();
            currencyRepoMock
                .Setup(crm => crm.All())
                .Returns(new List<Currency> {currency1, currency2, currency3 }
                                    .AsQueryable()
                                    .BuildMock()
                                    .Object);

            var sut = new CurrencyServices(currencyRepoMock.Object);
            //Act
            var currencyNames = await sut.GetCurrenciesAsync();
            //Assert
            Assert.IsTrue(currencyNames.Contains(currency1.CurrencyName));
            Assert.IsTrue(currencyNames.Contains(currency2.CurrencyName));
            Assert.IsTrue(currencyNames.Contains(currency3.CurrencyName));
        }
    }
}
