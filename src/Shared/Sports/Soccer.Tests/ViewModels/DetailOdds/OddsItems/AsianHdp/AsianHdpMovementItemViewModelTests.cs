namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems.AsianHdp
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using NSubstitute;
    using System.Collections.Generic;
    using Xunit;

    public class AsianHdpMovementItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IOddsMovement oddsMovement;

        public AsianHdpMovementItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            oddsMovement = Substitute.For<IOddsMovement>();
            oddsMovement.HomeScore.Returns(1);
            oddsMovement.AwayScore.Returns(0);
            oddsMovement.IsMatchStarted.Returns(true);
            oddsMovement.UpdateTime.Returns(new System.DateTime(2018, 7, 20, 12, 55, 00));
            oddsMovement.BetOptions.Returns(new List<BetOptionOdds>
            {
                new BetOptionOdds{ Type = "home", LiveOdds = 5.000m, OpeningOdds = 4.900m, OptionValue = "1", OddsTrend = OddsTrend.Up },               
                new BetOptionOdds{ Type = "away", LiveOdds = 2.500m, OpeningOdds = 2.800m, OptionValue = "1", OddsTrend = OddsTrend.Down }
            });

            this.baseFixture = baseFixture;
        }

        [Fact]
        public void HomeLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("5.00", viewModel.HomeOdds);
        }

        [Fact]
        public void OptionValue_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("1", viewModel.OptionValue);
        }

        [Fact]
        public void AwayLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("2.50", viewModel.AwayOdds);
        }

        [Fact]
        public void Score_MatchStarted_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

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
            var viewModel = new AsianHdpMovementItemViewModel(oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Empty(viewModel.MatchScore);
        }
    }
}
