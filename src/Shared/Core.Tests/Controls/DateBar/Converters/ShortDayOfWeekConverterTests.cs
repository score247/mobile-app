namespace LiveScore.Core.Tests.Controls.DateBar.Converters
{
    using System;
    using LiveScore.Core.Controls.DateBar.Converters;
    using Xunit;

    public class ShortDayOfWeekConverterTests
    {
        private readonly ShortDayOfWeekConverter converter;

        public ShortDayOfWeekConverterTests()
        {
            converter = new ShortDayOfWeekConverter();
        }

        [Fact]
        public void Convert_DayOfWeek_ReturnCorrectFormat()
        {
            // Arrange
            const string expected = "MON";

            // Act
            var actual = converter.Convert(new DateTime(2019, 4, 22).DayOfWeek, null, null, null);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBack_DayOfWeek_ReturnCorrectFormat()
        {
            // Arrange
            var expected = new DateTime(2019, 4, 22).DayOfWeek;

            // Act
            var actual = converter.ConvertBack(expected, null, null, null);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}