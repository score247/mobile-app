namespace LiveScore.Core.Tests.Services
{
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models;
    using LiveScore.Core.Services;
    using Xunit;

    public class SportServiceTests
    {
        [Fact]
        public void GetSportItems_Always_GetExpectedSportItems()
        {
            // Arrange
            var sportService = new SportService();
            var comparer = new CompareLogic();

            // Act
            var actual = sportService.GetSportItems();

            // Assert
            var expectedSportItem = new List<SportItem>
            {
                new SportItem { Type = SportType.Soccer },
                new SportItem { Type = SportType.Basketball  }
            };
            Assert.True(comparer.Compare(expectedSportItem, actual).AreEqual);
        }
    }
}