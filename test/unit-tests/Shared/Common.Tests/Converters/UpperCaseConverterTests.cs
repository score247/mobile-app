namespace LiveScore.Common.Tests.Converters
{
    using LiveScore.Common.Converters;
    using Xunit;

    public class UpperCaseConverterTests
    {
        private readonly UpperCaseConverter converter;

        public UpperCaseConverterTests()
        {
            converter = new UpperCaseConverter();
        }

        [Theory]
        [InlineData("upper")]
        [InlineData("Upper")]
        public void Convert_ReturnUpperString(string value)
        {
            // Arrange
            var expected = "UPPER";

            // Act
            var result = converter.Convert(value, null, null, null);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertBack_ReturnString()
        {
            // Arrange
            var expected = "UPPER";

            // Act
            var result = converter.ConvertBack("UPPER", null, null, null);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
