namespace Soccer.Tests.Converters
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Converters;
    using Xamarin.Forms;
    using Xunit;

    public class OddsStatusConverterTests : IClassFixture<ResourcesFixture>
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Convert_ValueIsNullOrWhiteSpace_ReturnNeutralAsDefault(string value)
        {
            // Arrange
            var converter = new OddsStatusConverter();

            // Act
            var actual = converter.Convert(value, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#1D2133"), actual);
        }

        [Fact]
        public void Convert_ValueIsUp_ReturnRedColor()
        {
            // Arrange
            var converter = new OddsStatusConverter();

            // Act
            var actual = converter.Convert(OddsTrend.Up.Value.ToString(), null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#FF222C"), actual);
        }

        [Fact]
        public void Convert_ValueIsDown_ReturGreenColor()
        {
            // Arrange
            var converter = new OddsStatusConverter();

            // Act
            var actual = converter.Convert(OddsTrend.Down.Value.ToString(), null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#66FF59"), actual);
        }
    }
}