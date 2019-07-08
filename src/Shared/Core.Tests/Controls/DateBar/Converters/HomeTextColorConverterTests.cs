namespace LiveScore.Core.Tests.Controls.DateBar.Converters
{
    using LiveScore.Core.Controls.DateBar.Converters;
    using LiveScore.Core.Tests.Fixtures;
    using Xamarin.Forms;
    using Xunit;

    public class HomeTextColorConverterTests : IClassFixture<ResourcesFixture>
    {
        private HomeTextColorConverter CreateHomeTextColorConverter()
        {
            return new HomeTextColorConverter();
        }

        [Fact]
        public void Convert_SelectedHome_ReturnPrimaryAccentColor()
        {
            // Arrange
            var unitUnderTest = CreateHomeTextColorConverter();
            const bool selectedHome = true;

            // Act
            var result = unitUnderTest.Convert(
                selectedHome,
                null,
                null,
                null);

            // Assert
            Assert.Equal(Color.FromHex("#F24822"), result);
        }

        [Fact]
        public void Convert_UnselectedHome_ReturnFouthTextColor()
        {
            // Arrange
            var unitUnderTest = CreateHomeTextColorConverter();
            const bool selectedHome = false;

            // Act
            var result = unitUnderTest.Convert(
                selectedHome,
                null,
                null,
                null);

            // Assert
            Assert.Equal(Color.FromHex("#939393"), result);
        }

        [Fact]
        public void ConvertBack_ShouldReturnSame()
        {
            // Arrange
            var unitUnderTest = CreateHomeTextColorConverter();
            const bool selectedHome = true;

            // Act
            var result = unitUnderTest.ConvertBack(
                selectedHome,
                null,
                null,
                null);

            // Assert
            Assert.True((bool)result);
        }
    }
}