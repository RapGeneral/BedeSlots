using BedeSlots.ViewModels.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BedeSlots.Tests.Enums
{
    [TestClass]
    public class GameItemChanceOutOf100Should
    {
        [TestMethod]
        public void ContainItemsMoreOrEqualTo0()
        {
            //Arrange
            var enumValues = Enum.GetValues(typeof(GameItemChanceOutOf100));
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
        public void HaveAllItemsSummingUpTo100()
        {
            //Arrange
            //The enum represents the chances of each items dropping
            var enumValues = Enum.GetValues(typeof(GameItemChanceOutOf100));
            int sum = 0;
            //Act
            foreach (int value in enumValues)
            {
                sum += value;
            }
            //Assert
            Assert.IsTrue(sum == 100);
        }
    }
}
