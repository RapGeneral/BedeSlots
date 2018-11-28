using BedeSlots.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BedeSlots.Tests.Services.SlotGamesServicesTests
{
    [TestClass]
    public class RunShould
    {
        [DataTestMethod]
        [DataRow(2, 3)]
        [DataRow(3, 2)]
        [DataRow(2, 2)]
        public void ThrowArgumentExpcetion_WhenTheMatrixIsSamllerThan3x3(int n, int m)
        {
            //Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var sut = new SlotGamesServices(memoryCache);
            //Act && Assert
            Assert.ThrowsException<ArgumentException>(() => sut.Run(n, m));
        }

        [DataTestMethod]
        [DataRow(3, 3)]
        [DataRow(3, 5)]
        [DataRow(5, 7)]
        public void CreateNxMFullMatrix(int n, int m)
        {
            //Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var sut = new SlotGamesServices(memoryCache);
            bool hasCorrectDimentions = true;
            bool doesntHave0Items = true; 
            //0 Items will check two things:
            //First, if every cell is populated
            //Second, if there are any items, whose values are 0 (for whatever reason...)
            //Act
            var result = sut.Run(n, m);
            //Assert
            if(result.Count != n)
            {
                hasCorrectDimentions = false;
            }
            for (int i = 0; i < n; i++)
            {
                if(result[i].Count != m)
                {
                    hasCorrectDimentions = false;
                    break;
                }
                for (int j = 0; j < m; j++)
                {
                    if((int)result[i][j] == 0)
                    {
                        doesntHave0Items = false;
                    }
                }
            }
            Assert.IsTrue(hasCorrectDimentions);
            Assert.IsTrue(doesntHave0Items);
        }
    }
}
