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
            Assert.Equal("en-US", actual.Value);
            Assert.Equal("English", actual.DisplayName);
        }

        [Fact]
        public void Vietnamese_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.Vietnamese;

            // Assert
            Assert.Equal("vi-VN", actual.Value);
            Assert.Equal("Vietnamese", actual.DisplayName);
        }

        [Fact]
        public void Thailand_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.Thailand;

            // Assert
            Assert.Equal("th-TH", actual.Value);
            Assert.Equal("Thailand", actual.DisplayName);
        }

        [Fact]
        public void Indonesia_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.Indonesia;

            // Assert
            Assert.Equal("id-ID", actual.Value);
            Assert.Equal("Indonesia", actual.DisplayName);
        }

        [Fact]
        public void TraditionalChinese_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.TraditionalChinese;

            // Assert
            Assert.Equal("zh-TW", actual.Value);
            Assert.Equal("Traditional Chinese", actual.DisplayName);
        }

        [Fact]
        public void SimplifiedChinese_Always_CreateCorrectType()
        {
            // Act
            var actual = Languages.SimplifiedChinese;

            // Assert
            Assert.Equal("zh-CN", actual.Value);
            Assert.Equal("Simplified Chinese", actual.DisplayName);
        }
    }
}