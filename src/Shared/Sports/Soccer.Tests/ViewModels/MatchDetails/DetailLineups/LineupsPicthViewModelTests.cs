using AutoFixture;
using LiveScore.Common.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsPicthViewModelTests
    {
        private readonly Fixture fixture;

        public LineupsPicthViewModelTests()
        {
            fixture = new Fixture();
        }

        [Theory]
        [InlineData(1600, 4, 593)]
        [InlineData(1600, 1, 2375)]
        public void Init_CorrectPitchViewHeight(double deviceWidth, double deviceDensity, double expectedPitchViewHeight)
        {
            // Arrange
            var deviceInfor = Substitute.For<IDeviceInfo>();
            deviceInfor.Width.Returns(deviceWidth);
            deviceInfor.Density.Returns(deviceDensity);
            var lineupsGroup = new LineupsPicthViewModel(
                "",
                deviceInfor,
                "",
                "",
                "",
                "");

            // Act
            var pitchViewHeight = lineupsGroup.PitchViewHeight;

            // Assert
            Assert.Equal(expectedPitchViewHeight, pitchViewHeight);
        }

        [Fact]
        public void Init_CorrectConstructorValues()
        {
            // Arrange
            var deviceInfor = Substitute.For<IDeviceInfo>();
            var pitchView = "this is pitch view";
            var expectedPitchView = "<html><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><body style=\"padding: 0; margin: 0;\">this is pitch view</body></html>";
            var lineupsGroup = new LineupsPicthViewModel(
                pitchView,
                deviceInfor,
                "home",
                "4-3-3",
                "away",
                "4-1-4-1");

            // Act

            // Assert
            Assert.Equal(expectedPitchView, lineupsGroup.LineupsPitchView.Html);
            Assert.Equal("HOME", lineupsGroup.HomeName);
            Assert.Equal("4 - 3 - 3", lineupsGroup.HomeFormation);
            Assert.Equal("AWAY", lineupsGroup.AwayName);
            Assert.Equal("4 - 1 - 4 - 1", lineupsGroup.AwayFormation);
        }
    }
}