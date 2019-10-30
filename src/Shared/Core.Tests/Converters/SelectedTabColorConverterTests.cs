using LiveScore.Core.Converters;
using LiveScore.Core.Tests.Fixtures;
using Xamarin.Forms;
using Xunit;

namespace LiveScore.Core.Tests.Converters
{
    public class SelectedTabColorConverterTests : IClassFixture<ResourcesFixture>
    {
        [Fact]
        public void Convert_ValueIsNull_ReturnNeutralAsDefault()
        {
            // Arrange
            var converter = new SelectedTabColorConverter();

            // Act
            var actual = converter.Convert(null, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#fff"), actual);
        }

        [Fact]
        public void Convert_Selected_ReturnRedColor()
        {
            // Arrange
            var converter = new SelectedTabColorConverter();

            // Act
            var actual = converter.Convert(true, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#3EC28F"), actual);
        }

        [Fact]
        public void Convert_Unselected_ReturGreenColor()
        {
            // Arrange
            var converter = new SelectedTabColorConverter();

            // Act
            var actual = converter.Convert(false, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#fff"), actual);
        }
    }
}