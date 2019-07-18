namespace LiveScore.Core.Tests.Converters
{
    using LiveScore.Core.Converters;
    using LiveScore.Core.Tests.Fixtures;
    using Xamarin.Forms;
    using Xunit;

    public class SelectedTabBackgroundColorConverterTests : IClassFixture<ResourcesFixture>
    {
        [Fact]
        public void Convert_ValueIsNull_ReturnNeutralAsDefault()
        {
            // Arrange            
            var converter = new SelectedTabBackgroundColorConverter();

            // Act
            var actual = converter.Convert(null, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#0F111C"), actual);
        }

        [Fact]
        public void Convert_Selected_ReturnRedColor()
        {
            // Arrange            
            var converter = new SelectedTabBackgroundColorConverter();

            // Act
            var actual = converter.Convert(true, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#1A1C28"), actual);
        }

        [Fact]
        public void Convert_Unselected_ReturGreenColor()
        {
            // Arrange            
            var converter = new SelectedTabBackgroundColorConverter();

            // Act
            var actual = converter.Convert(false, null, null, null);

            // Assert
            Assert.Equal(Color.FromHex("#0F111C"), actual);
        }
    }
}
