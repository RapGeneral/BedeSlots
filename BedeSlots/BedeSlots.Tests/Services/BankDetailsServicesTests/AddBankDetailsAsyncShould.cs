using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.MappingProvider;
using BedeSlots.Services;
using BedeSlots.Services.Utilities;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Tests.Services.BankDetailsServicesTests
{
    [TestClass]
    public class AddBankDetailsAsyncShould
    {
        [TestMethod]
        public async Task ShouldThrow_WhenItExists()
        {
            var number = "1111222233334444";
            var cvv = 123;
            var userid = "peshkonti";

            var bankDetailsRepoMock = new Mock<IRepository<BankDetails>>();
            bankDetailsRepoMock.Setup(bdr => bdr.All())
                .Returns(new List<BankDetails> { new BankDetails { Cvv = cvv, Number = number } }
                .AsQueryable()
                .BuildMock()
                .Object);                

            var mappingProviderMock = new Mock<IMappingProvider>();
            var dateTimeWrapperMock = new Mock<IDateTimeWrapper>();

            var sut = new BankDetailsServices(bankDetailsRepoMock.Object, mappingProviderMock.Object, dateTimeWrapperMock.Object);            

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.AddBankDetailsAsync(number, cvv, DateTime.Now, userid));
        }

        [TestMethod]
        public async Task ShouldCreate_WhenItDoesntExists()
        {
            var number = "1111222233334444";
            var cvv = 123;
            var userid = "peshkonti";
            DateTime expiryDate = DateTime.Parse("11/2017");

            var bankDetailsRepoMock = new Mock<IRepository<BankDetails>>();
            bankDetailsRepoMock.Setup(bdr => bdr.All())
                .Returns(new List<BankDetails>()
                .AsQueryable()
                .BuildMock()
                .Object);

            var dateTimeWrapperMock = new Mock<IDateTimeWrapper>();

            BankDetails mapInput = null;
            var mappingProviderMock = new Mock<IMappingProvider>();
            mappingProviderMock
                .Setup(mpm => mpm.MapTo<BankDetailsViewModel>(It.IsAny<BankDetails>()))
                .Callback<object>(inputArg => mapInput = inputArg as BankDetails);

            var sut = new BankDetailsServices(bankDetailsRepoMock.Object, mappingProviderMock.Object, dateTimeWrapperMock.Object);

            var result = await sut.AddBankDetailsAsync(number, cvv, expiryDate, userid);

            Assert.IsTrue(mapInput.Cvv == cvv);
            Assert.IsTrue(mapInput.Number == number);

        }
    }
}
