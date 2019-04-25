using LiveScore.Core.Controls.DateBar.Converters;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Core.Tests.Mocks;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xunit;

namespace LiveScore.Core.Tests.Controls.DateBar.Converters
{
    public class HomeTextColorConverterTests: IClassFixture<ResourcesFixture>
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
            var selectedHome = true;            

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
            var selectedHome = false;            

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
        public void ConvertBack_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateHomeTextColorConverter();
            var selectedHome = true;            

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
