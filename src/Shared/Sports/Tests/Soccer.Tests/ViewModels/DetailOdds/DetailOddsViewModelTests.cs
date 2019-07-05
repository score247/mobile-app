namespace Soccer.Tests.ViewModels.DetailOdds
{
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.ViewModels.DetailOdds;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using NSubstitute;
    using Prism.Navigation;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xunit;


    public class DetailOddsViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly DetailOddsViewModel viewModel;
        private readonly IOddsService oddsService;
        private readonly CompareLogic comparer;
        private readonly ILocalStorage localStorage;
        private readonly ILoggingService loggingService;

        private const string matchId = "sr:match:1";

        public DetailOddsViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            localStorage = Substitute.For<ILocalStorage>();
            loggingService = Substitute.For<ILoggingService>();
            oddsService = Substitute.For<IOddsService>();
            
            baseFixture.DependencyResolver.Resolve<IOddsService>("1").Returns(oddsService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>("1").Returns(loggingService);

            viewModel = new DetailOddsViewModel(
                matchId,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver);

            var parameters = new NavigationParameters { { "MatchId", matchId } };
            viewModel.OnNavigatingTo(parameters);
        }

        private MatchOdds CreateOdds()
        => new MatchOdds
        {
            MatchId = matchId,
            BetTypeOddsList = new List<BetTypeOdds>
            {
                CreateBetTypeOdds()
            }
        };

        private BetTypeOdds CreateBetTypeOdds() => new BetTypeOdds
        {
            Bookmaker = new Bookmaker { Id = "sr:book:1", Name = "Bet188Com" },
            BetOptions = new List<BetOptionOdds>
                    {
                        new BetOptionOdds{ Type = "home", LiveOdds = 5.0m, OpeningOdds = 4.9m, OddsTrend = OddsTrend.Up },
                        new BetOptionOdds{ Type = "draw", LiveOdds = 3.2m, OpeningOdds = 3.2m, OddsTrend = OddsTrend.Neutral },
                        new BetOptionOdds{ Type = "away", LiveOdds = 2.5m, OpeningOdds = 2.8m, OddsTrend = OddsTrend.Down }
                    }
        };

        [Fact]
        public void OnAppearing_Always_LoadOdds()
        {
            // Arrange
            oddsService.GetOdds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(CreateOdds());
            var expectedViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(BetType.OneXTwo, CreateBetTypeOdds(), viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.IsNotLoading);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
            Assert.True(comparer.Compare(expectedViewModels, viewModel.BetTypeOdds).AreEqual);
        }

        [Fact]
        public void OnAppearing_NoData()
        {
            // Arrange           

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.False(viewModel.HasData);
            Assert.True(viewModel.NoData);
            Assert.True(viewModel.IsNotLoading);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
        }
    }    
}
