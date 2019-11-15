using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OneXTwo;
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
                new BetOptionOdds( "home", 5.000m, 4.900m, "1.5", "1.25", OddsTrend.Up ),
                new BetOptionOdds( "draw", 3.200m, 3.200m, "3.2", "3.2", OddsTrend.Neutral ),
                new BetOptionOdds( "away", 2.500m, 2.800m, "2.5", "2.8", OddsTrend.Down )
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
