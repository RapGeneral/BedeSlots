using BedeSlots.ViewModels.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BedeSlots.Tests.GlobalViewModels.Enums
{
    [TestClass]
    public class GameItemCoeffsOutOf10Should
    {
        [TestMethod]
        public void ContainItemsLessOrEqualTo10()
        {
            //Arrange
            var enumValues = Enum.GetValues(typeof(GameItemCoeffsOutOf10));
            bool allLessOrEqualTo10 = true;
            //Act
            foreach (int value in enumValues)
            {
                if(value > 10)
                {
                    allLessOrEqualTo10 = false;
                    break;
                }
            }
            //Assert
            Assert.IsTrue(allLessOrEqualTo10);
        }

        [TestMethod]
        public void ContainItemsMoreOrEqualTo0()
        {
            //Arrange
            var enumValues = Enum.GetValues(typeof(GameItemCoeffsOutOf10));
            bool allMoreOrEqualTo0 = true;
            //Act
            foreach (int value in enumValues)
            {
                if (value < 0)
                {
                    allMoreOrEqualTo0 = false;
                    break;
                }
            }
            //Assert
            Assert.IsTrue(allMoreOrEqualTo0);
        }
        [TestMethod]
        public void HaveAllTheSameMembersAsTheChanceEnum()
        {
            //Arrange
            var enumCoeffValues = Enum.GetValues(typeof(GameItemCoeffsOutOf10)).Cast<GameItemCoeffsOutOf10>().ToList();
            var enumChanceValues = Enum.GetValues(typeof(GameItemChanceOutOf100)).Cast<GameItemChanceOutOf100>().ToList();
            bool allHaveSameNames = true;
            //Act
            foreach (var value in enumCoeffValues)
            {
                if (!enumChanceValues.Any(ecv => ecv.ToString().ToLower() == value.ToString().ToLower()))
                {
                    allHaveSameNames = false;
                    break;
                }
            }
            //Assert
            Assert.IsTrue(allHaveSameNames);
        }
    }
}
