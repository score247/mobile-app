namespace LiveScore.Core.Tests.Models.Matches
{
    using LiveScore.Core.Models.Matches;
    using System;
    using Xunit;

    public class MatchTests
    {
        [Fact]
        public void DisplayLocalTime_Always_GetCorrectFormat()
        {
            // Arrange
            var match = new Match();
            match.EventDate = new DateTime(2019, 2, 2, 12, 30, 30);

            // Act
            var actual = match.DisplayLocalTime;

            // Assert
            Assert.Equal("12:30", actual);
        }
    }
}