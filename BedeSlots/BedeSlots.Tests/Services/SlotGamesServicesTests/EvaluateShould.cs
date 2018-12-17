using BedeSlots.Services;
using BedeSlots.ViewModels.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BedeSlots.Tests.Services.SlotGamesServicesTests
{
    [TestClass]
    public class EvaluateShould
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
            var matrix = new List<List<GameItemChanceOutOf100>>();
            for (int i = 0; i < n; i++)
            {
                var row = new List<GameItemChanceOutOf100>();
                for (int j = 0; j < m; j++)
                {
                    row.Add(GameItemChanceOutOf100.Apple);
                }
                matrix.Add(row);
            }
            //Act && Assert
            Assert.ThrowsException<ArgumentException>(() => sut.Evaluate(matrix));
        }
        //Basically data rows with arrays.
        private static IEnumerable<object[]> FirstSlotMatrix =>
            new List<object[]> {
                new object[] {new List<List<GameItemChanceOutOf100>> {
                    new List<GameItemChanceOutOf100>{ GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Apple},
                    new List<GameItemChanceOutOf100>{ GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.Pen},
                    new List<GameItemChanceOutOf100>{GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.PPAP } },
                    (3*(int)Enum.Parse(typeof(GameItemCoeffsOutOf10), GameItemChanceOutOf100.Apple.ToString()))/10.0M//Coeff actual value
                },
                new object[] {new List<List<GameItemChanceOutOf100>> {
                    new List<GameItemChanceOutOf100>{ GameItemChanceOutOf100.PPAP, GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Apple},
                    new List<GameItemChanceOutOf100>{ GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.Pen},
                    new List<GameItemChanceOutOf100>{GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.PPAP } },
                    (2*(int)Enum.Parse(typeof(GameItemCoeffsOutOf10), GameItemChanceOutOf100.Apple.ToString()))/10.0M//Coeff actual value
                },
                new object[] {new List<List<GameItemChanceOutOf100>>  {
                    new List<GameItemChanceOutOf100> { GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Apple},
                    new List<GameItemChanceOutOf100> { GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.PPAP},
                    new List<GameItemChanceOutOf100> {GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.PPAP } },
                    (5*(int)Enum.Parse(typeof(GameItemCoeffsOutOf10), GameItemChanceOutOf100.Apple.ToString())+
                    (int)Enum.Parse(typeof(GameItemCoeffsOutOf10), GameItemChanceOutOf100.PPAP.ToString()))/10.0M//coeff actual value
                },
                new object[] {new List<List<GameItemChanceOutOf100>>  {
                    new List<GameItemChanceOutOf100> { GameItemChanceOutOf100.PPAP, GameItemChanceOutOf100.PPAP, GameItemChanceOutOf100.PPAP},
                    new List<GameItemChanceOutOf100> { GameItemChanceOutOf100.PPAP, GameItemChanceOutOf100.PPAP, GameItemChanceOutOf100.PPAP},
                    new List<GameItemChanceOutOf100> {GameItemChanceOutOf100.Apple, GameItemChanceOutOf100.Pineapple, GameItemChanceOutOf100.PPAP } },
                    (6*(int)Enum.Parse(typeof(GameItemCoeffsOutOf10), GameItemChanceOutOf100.PPAP.ToString()))/10.0M//coeff actual value
                }
            };
        [TestMethod]
        [DynamicData(nameof(FirstSlotMatrix))]
        public void CorrectlyCalculateCoefficients(List<List<GameItemChanceOutOf100>>slotMatrix, decimal actualCoeff)
        {
            //Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var sut = new SlotGamesServices(memoryCache);
            //Act
            var coeff = sut.Evaluate(slotMatrix);
            //Assert
            Assert.IsTrue(coeff == actualCoeff);
        }
    }
}
