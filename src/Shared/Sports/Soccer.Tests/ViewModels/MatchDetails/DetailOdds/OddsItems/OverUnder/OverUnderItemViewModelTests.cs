namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OverUnder;
    using NSubstitute;
    using Xunit;

    public class OverUnderItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IBetTypeOdds betTypeOdds;

        public OverUnderItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            betTypeOdds = Substitute.For<IBetTypeOdds>();
            betTypeOdds.Bookmaker.Returns(new Bookmaker ( "sr:book1", "book1" ));
            betTypeOdds.BetOptions.Returns(new List<BetOptionOdds>
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
            var viewModel = new OverUnderItemViewModel(betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("5.00", viewModel.OverLiveOdds);
        }

        [Fact]
        public void OptionValue_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OverUnderItemViewModel(betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("1.25", viewModel.OpeningOverOptionValue);
            Assert.Equal("1.5", viewModel.LiveOverOptionValue);
        }

        [Fact]
        public void UnderLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new OverUnderItemViewModel(betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("2.50", viewModel.UnderLiveOdds);
        }
    }
}
