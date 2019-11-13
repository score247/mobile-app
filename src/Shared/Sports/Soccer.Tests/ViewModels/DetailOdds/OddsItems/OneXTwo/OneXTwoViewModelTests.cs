namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OneXTwo;
    using NSubstitute;
    using Xunit;

    public class OneXTwoViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IBetTypeOdds betTypeOdds;

        public OneXTwoViewModelTests(ViewModelBaseFixture baseFixture)
        {
            betTypeOdds = Substitute.For<IBetTypeOdds>();
            betTypeOdds.Bookmaker.Returns(new Bookmaker ( "sr:book:1",  "book1" ));
            betTypeOdds.BetOptions.Returns(new List<BetOptionOdds>
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
