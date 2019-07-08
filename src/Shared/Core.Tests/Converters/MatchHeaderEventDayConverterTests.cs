namespace LiveScore.Core.Tests.Converters
{
    using LiveScore.Core.Converters;
    using Xunit;

    public class MatchHeaderEventDayConverterTests
    {
        private readonly MatchHeaderEventDayConverter converter;

        public MatchHeaderEventDayConverterTests()
        {
            converter = new MatchHeaderEventDayConverter();
        }

        [Fact]
        public void Convert_ShouldReturnShortDatime()
        {
            // Arrange
            var obj = new { Day = "21", Month = "4", Year = "2019" };
            const string expected = "21 Apr";

            // Act
            var actual = converter.Convert(obj, null, null, null);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBack_ShouldReturnShortDatime()
        {
            // Arrange
            const string value = "21 Apr";

            // Act
            var actual = converter.ConvertBack(value, null, null, null);

            // Assert
            Assert.Equal(value, actual);
        }
    }
}