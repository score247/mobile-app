namespace LiveScore.Core.Tests.Enumerations
{
    using LiveScore.Core.Enumerations;
    using Xunit;

    public class SportTypesTests
    {
        [Fact]
        public void Soccer_Always_CreateCorrectType()
        {
            // Act
            var actual = SportType.Soccer;

            // Assert
            Assert.Equal(1, actual.Value);
            Assert.Equal("Soccer", actual.DisplayName);
        }

        [Fact]
        public void Basketball_Always_CreateCorrectType()
        {
            // Act
            var actual = SportType.Basketball;

            // Assert
            Assert.Equal(2, actual.Value);
            Assert.Equal("Basketball", actual.DisplayName);
        }

        [Fact]
        public void ESport_Always_CreateCorrectType()
        {
            // Act
            var actual = SportType.ESport;

            // Assert
            Assert.Equal(3, actual.Value);
            Assert.Equal("ESport", actual.DisplayName);
        }

        [Fact]
        public void Constructor_DoNothing()
        {
            // Act
            new SportType();

            // Assert
            Assert.True(true);
        }
    }
}