namespace LiveScore.Common.Tests.Converters
{
    using LiveScore.Common.Converters;
    using Xunit;

    public class TextNotEmptyConverterTests
    {
        [Fact]
        public void Convert_ValueIsNotNullOrEmpty_ReturnTrue()
        {
            // Arrange
            var textNotEmtpyConverter = new TextNotEmptyConverter();

            // Act
            var actual = textNotEmtpyConverter.Convert("test", null, null, null);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Convert_ValueIsNotNull_ReturnFalse()
        {
            // Arrange
            var textNotEmtpyConverter = new TextNotEmptyConverter();

            // Act
            var actual = textNotEmtpyConverter.Convert(null, null, null, null);

            // Assert
            Assert.False(actual);
        }
    }
}