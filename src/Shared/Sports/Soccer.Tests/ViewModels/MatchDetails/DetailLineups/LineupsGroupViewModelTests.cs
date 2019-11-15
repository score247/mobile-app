using System.Collections.Generic;
using AutoFixture;
using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsGroupViewModelTests
    {
        private readonly Fixture fixture;

        public LineupsGroupViewModelTests()
        {
            fixture = new Fixture();
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("")]
        [InlineData(null)]
        public void Init_GroupName_CorrectGroupName(string groupName)
        {
            // Arrange
            var lineupsGroup = new LineupsGroupViewModel(groupName, new List<LineupsItemViewModel>());

            // Act
            var lineUpsGroupName = lineupsGroup.Name;

            // Assert
            Assert.Equal(groupName, lineUpsGroupName);
        }
    }
}