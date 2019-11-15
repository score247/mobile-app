namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems.OverUnder
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OverUnder;
    using NSubstitute;
    using Xunit;

    public class OverUnderMovementItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IOddsMovement oddsMovement;

        public OverUnderMovementItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            oddsMovement = Substitute.For<IOddsMovement>();
            oddsMovement.HomeScore.Returns(1);
            oddsMovement.AwayScore.Returns(0);
            oddsMovement.IsMatchStarted.Returns(true);
            oddsMovement.UpdateTime.Returns(new System.DateTime(2018, 7, 20, 12, 55, 00));
            oddsMovement.BetOptions.Returns(new List<BetOptionOdds>
            {
                new BetOptionOdds( "over", 5.000m, 4.900m, "1.5", "1.25", OddsTrend.Up ),
                new BetOptionOdds( "under", 2.500m, 2.800m, "1.8", "2", OddsTrend.Down )
            });

            this.baseFixture = baseFixture;
        }

        [Fact]
        public void OverLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OverUnderMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("5.00", viewModel.OverOdds);
        }

        [Fact]
        public void UnderLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OverUnderMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("2.50", viewModel.UnderOdds);
        }

        [Fact]
        public void OptionValue_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OverUnderMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("1.5", viewModel.OptionValue);
        }

        [Fact]
        public void Score_MatchStarted_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OverUnderMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

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
            var viewModel = new OverUnderMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Empty(viewModel.MatchScore);
        }
    }
}
