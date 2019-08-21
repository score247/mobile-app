namespace Soccer.Tests.ViewModels.DetailOdds
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core;
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
    using Xunit;

    public class DetailOddsViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private const string matchId = "sr:match:1";

        private readonly DetailOddsViewModel viewModel;
        private readonly IOddsService oddsService;
        private readonly CompareLogic comparer;
        private readonly ILoggingService mockLogService;
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver dependencyResolver;

        public DetailOddsViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            mockLogService = Substitute.For<ILoggingService>();
            oddsService = Substitute.For<IOddsService>();

            navigationService = baseFixture.NavigationService;
            dependencyResolver = baseFixture.DependencyResolver;

            baseFixture.DependencyResolver.Resolve<IOddsService>("1").Returns(oddsService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(mockLogService);

            viewModel = new DetailOddsViewModel(
                matchId,
                navigationService,
                baseFixture.DependencyResolver,
                null);

            var parameters = new NavigationParameters { { "MatchId", matchId } };
            viewModel.OnNavigatingTo(parameters);
        }

        private MatchOdds CreateOdds() => CreateOdds(BetType.AsianHDP.Value);

        private MatchOdds CreateOdds(int betTypeId)
            => new MatchOdds
            {
                MatchId = matchId,
                BetTypeOddsList = new List<BetTypeOdds>
                {
                    CreateBetTypeOdds(betTypeId)
                }
            };

        private BetTypeOdds CreateBetTypeOdds(int betTypeId) => new BetTypeOdds
        {
            Id = betTypeId,
            Bookmaker = new Bookmaker { Id = "sr:book:1", Name = "Bet188Com" },
            BetOptions = new List<BetOptionOdds>
            {
                new BetOptionOdds{ Type = "home", LiveOdds = 5.0m, OpeningOdds = 4.9m, OddsTrend = OddsTrend.Up },
                new BetOptionOdds{ Type = "draw", LiveOdds = 3.2m, OpeningOdds = 3.2m, OddsTrend = OddsTrend.Neutral },
                new BetOptionOdds{ Type = "away", LiveOdds = 2.5m, OpeningOdds = 2.8m, OddsTrend = OddsTrend.Down }
            }
        };

        [Fact]
        public void OnAppearing_NoData()
        {
            // Arrange

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.False(viewModel.HasData);
            Assert.True(viewModel.IsNotLoading);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
        }

        [Fact]
        public void OnAppearing_Always_LoadOdds()
        {
            // Arrange
            oddsService.GetOdds(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(CreateOdds());
            var expectedViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(BetType.AsianHDP, CreateBetTypeOdds(BetType.AsianHDP.Value), viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.IsNotLoading);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
            Assert.True(comparer.Compare(expectedViewModels, viewModel.BetTypeOddsItems).AreEqual);
        }

        [Fact]
        public async Task RefreshCommand_OnExecute_LoadOdds()
        {
            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await oddsService.Received(1).GetOdds(Arg.Any<string>(), Arg.Is(matchId), Arg.Is(3), Arg.Any<string>(), true);
        }

        [Fact]
        public async Task OnOddsTabClicked_OnExecute_LoadOdds()
        {
            // Act
            await viewModel.OnOddsTabClicked.ExecuteAsync("1");

            // Assert
            await oddsService.Received(1).GetOdds(Arg.Any<string>(), Arg.Is(matchId), Arg.Is(1), Arg.Any<string>(), false);
        }

        [Fact]
        public async Task IsOneXTwoSelected_SelectedOneXTwo_MustTrue()
        {
            // Act
            await viewModel.OnOddsTabClicked.ExecuteAsync("1");

            // Assert
            Assert.True(viewModel.IsOneXTwoSelected);
        }

        [Fact]
        public async Task IsOverUnderSelected_SelectedOverUnder_MustTrue()
        {
            // Act
            await viewModel.OnOddsTabClicked.ExecuteAsync("2");

            // Assert
            Assert.True(viewModel.IsOverUnderSelected);
        }

        [Fact]
        public async Task IsAsianHdpSelected_SelectedAsianHdp_MustTrue()
        {
            // Act
            await viewModel.OnOddsTabClicked.ExecuteAsync("3");

            // Assert
            Assert.True(viewModel.IsAsianHdpSelected);
        }

        [Fact]
        public async Task TappedOddsItemCommand_OnExecuting_CallNavigationService()
        {
            // Arrange
            oddsService.GetOdds(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(CreateOdds());
            await viewModel.RefreshCommand.ExecuteAsync();
            var oddsItemViewModel = new BaseItemViewModel(
                BetType.OneXTwo,
                CreateBetTypeOdds(BetType.OneXTwo.Value),
                navigationService,
                dependencyResolver);

            // Act
            await viewModel.TappedOddsItemCommand.ExecuteAsync(oddsItemViewModel);

            // Assert
            var navService = viewModel.NavigationService as FakeNavigationService;
            Assert.Equal("OddsMovementView" + viewModel.SettingsService.CurrentSportType.Value, navService.NavigationPath);
        }

        [Fact]
        public async Task TappedOddsItemCommand_NavigatedFailed_LogException()
        {
            // Arrange
            var mockNavigationService = Substitute.For<INavigationService>();
            mockNavigationService.NavigateAsync(Arg.Any<string>(), Arg.Any<INavigationParameters>())
                .Returns(new NavigationResult { Success = false, Exception = new InvalidOperationException("Cannot navigated") });

            var oddsViewModel = new DetailOddsViewModel(
                matchId,
                mockNavigationService,
                dependencyResolver,
                null);

            var parameters = new NavigationParameters { { "MatchId", matchId } };
            oddsViewModel.OnNavigatingTo(parameters);

            oddsService.GetOdds(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(CreateOdds());

            var oddsItemViewModel = new BaseItemViewModel(
                BetType.AsianHDP,
                CreateBetTypeOdds(BetType.AsianHDP.Value),
                navigationService,
                dependencyResolver);

            // Act
            await oddsViewModel.TappedOddsItemCommand.ExecuteAsync(oddsItemViewModel);

            // Assert
            await mockLogService.Received(1).LogErrorAsync(Arg.Any<InvalidOperationException>());
        }

        [Fact]
        public async Task DeserializeComparisonMessage_Null_LogException()
        {
            // Act
            await viewModel.DeserializeComparisonMessage(null);

            // Assert
            await mockLogService.Received(1).LogErrorAsync(Arg.Any<string>(), Arg.Any<Exception>());
        }

        [Fact]
        public async Task DeserializeComparisonMessage_NotNull_NotLogException()
        {
            // Act
            await viewModel.DeserializeComparisonMessage("");

            // Assert
            await mockLogService.DidNotReceive().LogErrorAsync(Arg.Any<string>(), Arg.Any<Exception>());
        }

        [Fact]
        public async Task HandleOddsComparisonMessage_NotCurentMatch_NotLoadOdds()
        {
            // Arrange 
            var oddsComparison = new MatchOddsComparisonMessage
            {
                MatchId = "sr:match:2",
                BetTypeOddsList = new List<BetTypeOdds> { CreateBetTypeOdds(BetType.AsianHDP.Value) }
            };

            // Act
            await viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert
            await oddsService.DidNotReceive().GetOdds(Arg.Any<string>(), Arg.Is(matchId), Arg.Is(1), Arg.Any<string>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task HandleOddsComparisonMessage_BetTypeOddsListIsNull_NotLoadOdds()
        {
            // Arrange 
            var oddsComparison = new MatchOddsComparisonMessage
            {
                MatchId = matchId,
                BetTypeOddsList = null
            };

            // Act
            await viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert
            await oddsService.DidNotReceive().GetOdds(Arg.Any<string>(), Arg.Is(matchId), Arg.Is(1), Arg.Any<string>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task HandleOddsComparisonMessage_NotSelectedBetType_NotLoadOdds()
        {
            // Arrange 
            var oddsComparison = new MatchOddsComparisonMessage
            {
                MatchId = matchId,
                BetTypeOddsList = new List<BetTypeOdds> { CreateBetTypeOdds(BetType.OneXTwo.Value) }
            };

            // Act
            await viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert
            await oddsService.DidNotReceive().GetOdds(Arg.Any<string>(), Arg.Is(matchId), Arg.Is(1), Arg.Any<string>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task HandleOddsComparisonMessage_LoadOdds_AddNew()
        {
            // Arrange 
            var oddsComparison = new MatchOddsComparisonMessage
            {
                MatchId = matchId,
                BetTypeOddsList = new List<BetTypeOdds> { CreateBetTypeOdds(BetType.AsianHDP.Value) }
            };

            // Act            
            await viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert
            await oddsService.Received(1).GetOdds(Arg.Any<string>(), Arg.Is(matchId), Arg.Is(BetType.AsianHDP.Value), Arg.Any<string>(), Arg.Any<bool>());
            Assert.True(viewModel.HasData);
            Assert.Single(viewModel.BetTypeOddsItems);
        }

        [Fact]
        public async Task HandleOddsComparisonMessage_LoadOdds_UpdateExisting()
        {
            // Arrange 
            oddsService
                .GetOdds(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(CreateOdds(BetType.OneXTwo.Value));

            await viewModel.OnOddsTabClicked.ExecuteAsync("1");

            var oddsComparison = new MatchOddsComparisonMessage
            {
                MatchId = matchId,
                BetTypeOddsList = new List<BetTypeOdds> { CreateBetTypeOdds(BetType.OneXTwo.Value) }
            };

            oddsComparison.BetTypeOddsList.First().BetOptions.First(x=>x.Type == "home").LiveOdds = 5.6m;

            // Act            
            await viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert            
            Assert.True(viewModel.HasData);
            Assert.Single(viewModel.BetTypeOddsItems);

            var itemViewModel = viewModel.BetTypeOddsItems.First() as OneXTwoItemViewModel;
            Assert.Equal("5.60", itemViewModel.HomeLiveOdds);
        }
    }
}