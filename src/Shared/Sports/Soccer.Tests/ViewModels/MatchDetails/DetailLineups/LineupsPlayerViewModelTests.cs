using AutoFixture;
using LiveScore.Core;
using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsPlayerViewModelTests
    {
        private readonly IDependencyResolver denpendacyResolver;
        private readonly Fixture fixture;

        public LineupsPlayerViewModelTests()
        {
            denpendacyResolver = Substitute.For<IDependencyResolver>();
            fixture = new Fixture();
        }

        [Theory]
        [InlineData("Aley", 0, "Kernal", 0)]
        [InlineData("Mitch", 10, "", 2)]
        [InlineData("", 0, "", 5)]
        public void Ini_CorrectNameAndJersey(string homePlayer, int homeJersey, string awayPlayer, int awayJersey)
        {
            // Arrange
            var lineupsPlayerViewModel = new LineupsPlayerViewModel(
                denpendacyResolver,
                homePlayer,
                awayPlayer,
                homeJersey,
                awayJersey,
                null,
                null);
            // Act

            // Assert
            Assert.Equal(homePlayer, lineupsPlayerViewModel.HomeName);
            Assert.Equal(homeJersey, lineupsPlayerViewModel.HomeJerseyNumber);
            Assert.Equal(awayPlayer, lineupsPlayerViewModel.AwayName);
            Assert.Equal(awayJersey, lineupsPlayerViewModel.AwayJerseyNumber);
        }
    }
}