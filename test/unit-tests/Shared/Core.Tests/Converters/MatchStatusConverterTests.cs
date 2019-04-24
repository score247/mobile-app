using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using Xunit;
namespace LiveScore.Core.Tests.Converters
{
    public class MatchStatusConverterTests
    {
        private readonly MatchStatusConverter converter;

        public MatchStatusConverterTests()
        {
            converter = new MatchStatusConverter();
        }

        [Fact]
        public void Convert_StatusIsFullTime_ReturnCorrectFormat()
        {
            // Arrange
            var expected = "FT";

            // Act
            var actual = converter.Convert(MatchStatus.FullTimeStatus, null, null, null);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_StatusIsLive_ReturnCorrectFormat()
        {
            // Arrange            

            // Act
            var actual = converter.Convert(MatchStatus.LiveStatus, null, null, null);

            // Assert
            Assert.Equal(MatchStatus.LiveStatus.ToString(), actual);
        }

        [Fact]
        public void Convert_StatusIsNotStarted_ReturnCorrectFormat()
        {
            // Arrange            

            // Act
            var actual = converter.Convert(MatchStatus.NotStartedStatus, null, null, null);

            // Assert
            Assert.Equal(MatchStatus.NotStartedStatus.ToString(), actual);
        }

        [Fact]
        public void Convert_StatusIsNull_ReturnEmpty()
        {
            // Arrange            

            // Act
            var actual = converter.Convert(null, null, null, null);

            // Assert
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ConvertBack_ReturnMatchStatus()
        {
            // Arrange            

            // Act
            var actual = converter.ConvertBack(MatchStatus.NotStartedStatus, null, null, null);

            // Assert
            Assert.IsType<MatchStatus>(actual);
        }
    }
}
