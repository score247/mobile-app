namespace LiveScore.Core.Tests.Converters
{
    using LiveScore.Core.Converters;
    using NSubstitute;
    using System;
    using System.Globalization;
    using Xunit;

    public class FirstCharConverterTests
    {
        [Fact]
        public void Convert_ValueIsNotNull_GetFirstCharWithUpperCase()
        {
            // Arrange
            var value = "Chelsea";
            var converter = new FirstCharConverter();

            // Act
            var actual = converter.Convert(value, null, null, null);

            // Assert
            Assert.Equal("C", actual);
        }

        [Fact]
        public void Convert_ValueIsNull_GetEmpty()
        {
            // Arrange
            const string value = "";
            var converter = new FirstCharConverter();

            // Act
            var actual = converter.Convert(value, null, null, null);

            // Assert
            Assert.Equal("", actual);
        }

        [Fact]
        public void ConvertBack_Always_ReturnValue()
        {
            // Arrange
            var converter = new FirstCharConverter();

            // Act
            var actual = converter.ConvertBack("A", null, null, null);

            // Assert
            Assert.Equal("A", actual);
        }
    }
}