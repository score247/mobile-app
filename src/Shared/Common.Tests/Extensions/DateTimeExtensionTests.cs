namespace LiveScore.Common.Tests.Extensions
{
    using System;
    using LiveScore.Common.Extensions;
    using Xunit;

    public class DateTimeExtensionTests
    {
        [Fact]
        public void Yesterday_ShouldReturnCorrectDate()
        {
            // Arrange
            var expected = DateTime.Today.AddDays(-1);

            // Act
            var actual = DateTimeExtension.Yesterday();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EndOfDay_ShouldReturnCorrectDate()
        {
            // Arrange
            var expected = DateTime.Today.AddDays(1).AddMilliseconds(-1);

            // Act
            var actual = DateTimeExtension.EndOfDay(DateTime.Today);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToApiFormat_ShouldReturnCorrectFormat()
        {
            // Arrange
            const string expected = "2019-04-22T12:00:00+00:00";

            // Act
            var actual = new DateTime(2019, 4, 22, 12, 0, 0, DateTimeKind.Utc).ToApiFormat();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToShortDayMonth_ShouldReturnCorrectFormat()
        {
            // Arrange
            const string expected = "22 Apr";

            // Act
            var actual = new DateTimeOffset(new DateTime(2019, 4, 22, 12, 0, 0, DateTimeKind.Utc)).ToDayMonth();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}