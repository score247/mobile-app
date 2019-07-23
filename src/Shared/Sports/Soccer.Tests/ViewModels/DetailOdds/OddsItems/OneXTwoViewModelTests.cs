namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using NSubstitute;
    using Xunit;

    public class OneXTwoViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IBetTypeOdds betTypeOdds;

        public OneXTwoViewModelTests(ViewModelBaseFixture baseFixture)
        {
            betTypeOdds = Substitute.For<IBetTypeOdds>();
            betTypeOdds.Bookmaker.Returns(new Bookmaker { Id = "book1", Name = "book1" });
            betTypeOdds.BetOptions.Returns(new List<BetOptionOdds>
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
            var viewModel = new OneXTwoItemViewModel(betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("5.00", viewModel.HomeLiveOdds);
        }

        [Fact]
        public void DrawLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OneXTwoItemViewModel(betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("3.20", viewModel.DrawLiveOdds);
        }

        [Fact]
        public void AwayLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OneXTwoItemViewModel(betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("2.50", viewModel.AwayLiveOdds);
        }
    }
}
