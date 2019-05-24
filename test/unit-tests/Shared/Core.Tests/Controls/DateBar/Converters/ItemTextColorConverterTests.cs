namespace LiveScore.Core.Tests.Controls.DateBar.Converters
{
    using System;
    using LiveScore.Core.Controls.DateBar.Converters;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Tests.Fixtures;
    using Xamarin.Forms;
    using Xunit;

    public class ItemTextColorConverterTests : IClassFixture<ResourcesFixture>
    {
        private ItemTextColorConverter CreateItemTextColorConverter()
        {
            return new ItemTextColorConverter();
        }

        [Fact]
        public void Convert_DateBarItemIsNull_ReturnFourthTextColor()
        {
            // Arrange
            var unitUnderTest = CreateItemTextColorConverter();

            // Act
            var result = unitUnderTest.Convert(
                null,
                null,
                null,
                null);

            // Assert
            Assert.Equal(Color.FromHex("#939393"), result);
        }

        [Fact]
        public void Convert_DateBarItemIsSelected_ReturnPrimaryAccentColor()
        {
            // Arrange
            var unitUnderTest = CreateItemTextColorConverter();
            var value = new DateBarItem { IsSelected = true };

            // Act
            var result = unitUnderTest.Convert(
                value,
                null,
                null,
                null);

            // Assert
            Assert.Equal(Color.FromHex("#F24822"), result);
        }

        [Fact]
        public void Convert_DateBarItemIsTodayAndNotSelected_ReturnWhiteColor()
        {
            // Arrange
            var unitUnderTest = CreateItemTextColorConverter();
            var value = new DateBarItem { Date = DateTime.Now };

            // Act
            var result = unitUnderTest.Convert(
                value,
                null,
                null,
                null);

            // Assert
            Assert.Equal(Color.White, result);
        }

        [Fact]
        public void Convert_DateBarItemIsNormal_ReturnFourthTextColor()
        {
            // Arrange
            var unitUnderTest = CreateItemTextColorConverter();
            var value = new DateBarItem { Date = DateTime.Today.AddDays(1) };

            // Act
            var result = unitUnderTest.Convert(
                value,
                null,
                null,
                null);

            // Assert
            Assert.Equal(Color.FromHex("#939393"), result);
        }

        [Fact]
        public void ConvertBack_ShouldReturnDateBarItem()
        {
            // Arrange
            var unitUnderTest = CreateItemTextColorConverter();

            // Act
            var result = unitUnderTest.ConvertBack(
                new DateBarItem { IsSelected = true },
                null,
                null,
                null
                );

            // Assert
            Assert.IsType<DateBarItem>(result);
        }
    }
}