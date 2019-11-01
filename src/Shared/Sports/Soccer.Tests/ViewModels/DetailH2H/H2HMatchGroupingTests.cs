using AutoFixture;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HMatchGroupingTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly Fixture fixture;

        public H2HMatchGroupingTests(ViewModelBaseFixture baseFixture)
        {
            fixture = baseFixture.CommonFixture.Specimens;
        }

        [Fact]
        public void Init_MatchIsNull()
        {
            // Arrange               

            // Act
            var matchGrouping = new H2HMatchGrouping(null);

            // Assert
            Assert.True(string.IsNullOrEmpty(matchGrouping.LeagueId));
        }

        [Fact]
        public void Equals_SameLeagueIdAndSeasonId_ReturnTrue()
        {
            // Arrange
            var match1 = fixture.Create<SoccerMatch>()
                .With(match => match.LeagueId, "sr:league:1")
                .With(match => match.LeagueSeasonId, "sr:season:1");

            var match2 = fixture.Create<SoccerMatch>()
                .With(match => match.LeagueId, "sr:league:1")
                .With(match => match.LeagueSeasonId, "sr:season:1");

            var matchGrouping1 = new H2HMatchGrouping(match1);
            var matchGrouping2 = new H2HMatchGrouping(match2);

            // Act
            var result = matchGrouping1.Equals(matchGrouping2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_SameLeagueIdAndDifferentSeasonId_ReturnFalse()
        {
            // Arrange
            var match1 = fixture.Create<SoccerMatch>()
                .With(match => match.LeagueId, "sr:league:1")
                .With(match => match.LeagueSeasonId, "sr:season:1");
           
            var match2 = fixture.Create<SoccerMatch>()
                .With(match => match.LeagueId, "sr:league:1")
                .With(match => match.LeagueSeasonId, "sr:season:2");

            var matchGrouping1 = new H2HMatchGrouping(match1);
            var matchGrouping2 = new H2HMatchGrouping(match2);

            // Act
            var result = matchGrouping1.Equals(matchGrouping2);

            // Assert
            Assert.False(result);
        }
    }
}
