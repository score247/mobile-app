using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    public class OneXTwoMovementItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IOddsMovement oddsMovement;

        public OneXTwoMovementItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            oddsMovement = Substitute.For<IOddsMovement>();
            oddsMovement.HomeScore.Returns(1);
            oddsMovement.AwayScore.Returns(0);
            oddsMovement.IsMatchStarted.Returns(true);
            oddsMovement.UpdateTime.Returns(new System.DateTime(2018, 7, 20, 12, 55, 00));
            oddsMovement.BetOptions.Returns(new List<BetOptionOdds>
            {
                new BetOptionOdds{ Type = "home", LiveOdds = 5.000m, OpeningOdds = 4.900m, OddsTrend = OddsTrend.Up },
                new BetOptionOdds{ Type = "draw", LiveOdds = 3.200m, OpeningOdds = 3.200m, OddsTrend = OddsTrend.Neutral },
                new BetOptionOdds{ Type = "away", LiveOdds = 2.500m, OpeningOdds = 2.800m, OddsTrend = OddsTrend.Down }
            });

            this.baseFixture = baseFixture;
        }

        [Fact]
        public void HomeLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OneXTwoMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("5.00", viewModel.HomeOdds);
        }

        [Fact]
        public void DrawLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OneXTwoMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("3.20", viewModel.DrawOdds);
        }

        [Fact]
        public void AwayLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OneXTwoMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("2.50", viewModel.AwayOdds);
        }

        [Fact]
        public void Score_MatchStarted_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OneXTwoMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("1 - 0", viewModel.MatchScore);
        }

        [Fact]
        public void Score_MatchNotStarted_GetExpectedFormat()
        {
            // Arrange         
            oddsMovement.IsMatchStarted.Returns(false);
            var viewModel = new OneXTwoMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Empty(viewModel.MatchScore);
        }
    }
}
