namespace LiveScore.Core.Tests.Enumerations
{
    using LiveScore.Core.Enumerations;
    using Xunit;

    public class LanguagesTests
    {
        [Fact]
        public void Constructor_DoNothing()
        {
            // Act
            new Languages();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void English_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.English;

            // Assert
            Assert.Equal(1, actual.Value);
            Assert.Equal("en-US", actual.DisplayName);
        }

        [Fact]
        public void Vietnamese_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.Vietnamese;

            // Assert
            Assert.Equal(2, actual.Value);
            Assert.Equal("vi-VN", actual.DisplayName);
        }

        [Fact]
        public void Thailand_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.Thailand;

            // Assert
            Assert.Equal(3, actual.Value);
            Assert.Equal("th-TH", actual.DisplayName);
        }

        [Fact]
        public void Indonesia_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.Indonesia;

            // Assert
            Assert.Equal(4, actual.Value);
            Assert.Equal("id-ID", actual.DisplayName);
        }

        [Fact]
        public void TraditionalChinese_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.TraditionalChinese;

            // Assert
            Assert.Equal(5, actual.Value);
            Assert.Equal("zh-TW", actual.DisplayName);
        }

        [Fact]
        public void SimplifiedChinese_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.SimplifiedChinese;

            // Assert
            Assert.Equal(6, actual.Value);
            Assert.Equal("zh-CN", actual.DisplayName);
        }
    }
}