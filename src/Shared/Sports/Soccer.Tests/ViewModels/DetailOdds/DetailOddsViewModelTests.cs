namespace Soccer.Tests.ViewModels.DetailOdds
{
    using System;
    using System.Collections.Generic;
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
    using LiveScore.Soccer.PubSubEvents.Odds;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.AsianHdp;
    using NSubstitute;
    using Prism.Events;
    using Prism.Navigation;
    using Xunit;

    public class DetailOddsViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private const string matchId = "sr:match:1";

        private readonly OddsViewModel viewModel;
        private readonly IOddsService oddsService;
        private readonly CompareLogic comparer;
        private readonly ILoggingService mockLogService;
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver dependencyResolver;
        private readonly IEventAggregator eventAggregator;

        public DetailOddsViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            mockLogService = Substitute.For<ILoggingService>();
            oddsService = Substitute.For<IOddsService>();
            eventAggregator = Substitute.For<IEventAggregator>();
            eventAggregator.GetEvent<ConnectionChangePubSubEvent>().Returns(new ConnectionChangePubSubEvent());
            eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Returns(new OddsComparisonPubSubEvent());

            navigationService = baseFixture.NavigationService;
            dependencyResolver = baseFixture.DependencyResolver;

            baseFixture.DependencyResolver.Resolve<IOddsService>("1").Returns(oddsService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(mockLogService);
            baseFixture.DependencyResolver.Resolve<IHubService>("1").Returns(baseFixture.HubService);

            viewModel = new OddsViewModel(
                matchId,
                MatchStatus.NotStarted,
                navigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);
        }

        [Fact]
        public async Task FirstLoadOrRefreshOddsAsync_Always_GetOddsAsync()
        {
            // Arrange
            oddsService.GetOddsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(StubOdds());
            var expectedViewModels = new List<BaseItemViewModel>
            {
                new BaseItemViewModel(BetType.AsianHDP, StubBetTypeOdds(BetType.AsianHDP.Value, BetType.AsianHDP.DisplayName), viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            // Act
            await viewModel.FirstLoadOrRefreshOddsAsync(BetType.AsianHDP, "decimal");

            // Assert
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.IsNotBusy);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsBusy);
            Assert.True(comparer.Compare(expectedViewModels, viewModel.BetTypeOddsItems).AreEqual);
        }

        [Fact]
        public async Task FirstLoadOrRefreshOddsAsync_NoData_HasDataShouldBeFalse()
        {
            // Arrange
            oddsService.GetOddsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>())
                .Returns(default(MatchOdds));

            // Act
            await viewModel.FirstLoadOrRefreshOddsAsync(BetType.AsianHDP, "decimal");

            // Assert
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task OnOddsTabClicked_OnExecute_LoadOdds()
        {
            // Act
            await viewModel.OnOddsTabClicked.ExecuteAsync("1");

            // Assert
            await oddsService.Received(1).GetOddsAsync(Arg.Any<string>(), Arg.Is(matchId), 1, Arg.Any<string>());
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
            oddsService.GetOddsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(StubOdds());
            await viewModel.RefreshCommand.ExecuteAsync();
            var oddsItemViewModel = new BaseItemViewModel(
                BetType.OneXTwo,
                StubBetTypeOdds(BetType.OneXTwo.Value, BetType.OneXTwo.DisplayName),
                navigationService,
                dependencyResolver);

            // Act
            await viewModel.TappedOddsItemCommand.ExecuteAsync(oddsItemViewModel);

            // Assert
            var navService = viewModel.NavigationService as FakeNavigationService;
            Assert.Equal("OddsMovementView" + viewModel.CurrentSportId, navService.NavigationPath);
        }

        [Fact]
        public async Task TappedOddsItemCommand_NavigatedFailed_LogException()
        {
            // Arrange
            var mockNavigationService = Substitute.For<INavigationService>();
            mockNavigationService.NavigateAsync(Arg.Any<string>(), Arg.Any<INavigationParameters>())
                .Returns(new NavigationResult { Success = false, Exception = new InvalidOperationException("Cannot navigated") });

            oddsService.GetOddsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(StubOdds());

            var oddsItemViewModel = new BaseItemViewModel(
                BetType.AsianHDP,
                StubBetTypeOdds(BetType.AsianHDP.Value, BetType.AsianHDP.DisplayName),
                navigationService,
                dependencyResolver);

            // Act
            await viewModel.TappedOddsItemCommand.ExecuteAsync(oddsItemViewModel);

            // Assert
            await mockLogService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
        }

        [Fact]
        public void HandleOddsComparisonMessage_LoadOdds_AddNew()
        {
            // Arrange
            var oddsComparison = new OddsComparisonMessage
            (
                1,
                "sr:match:1",
                new List<BetTypeOdds> { StubBetTypeOdds(BetType.AsianHDP.Value, BetType.AsianHDP.DisplayName) }
            );

            // Act
            viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert
            Assert.True(viewModel.HasData);
            Assert.Single(viewModel.BetTypeOddsItems);
        }

        [Fact]
        public async Task HandleOddsComparisonMessage_LoadOdds_UpdateExisting()
        {
            // Arrange
            await FirstLoad(BetType.AsianHDP);
            var oddsComparison = new OddsComparisonMessage
           (
               1,
               "sr:match:1",
               new List<BetTypeOdds> { StubBetTypeOdds(BetType.AsianHDP.Value, BetType.AsianHDP.DisplayName, 5.60m) }
           );

            // Act
            viewModel.HandleOddsComparisonMessage(oddsComparison);

            // Assert
            Assert.True(viewModel.HasData);
            Assert.Single(viewModel.BetTypeOddsItems);

            var itemViewModel = viewModel.BetTypeOddsItems.First() as AsianHdpItemViewModel;
            Assert.Equal("5.60", itemViewModel.HomeLiveOdds);
        }

        [Fact]
        public async Task OnResume_Always_LoadOdds()
        {
            // Arrange
            oddsService
                .GetOddsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>())
                .Returns(StubOdds(BetType.OneXTwo.Value, BetType.OneXTwo.DisplayName));

            // Act
            await viewModel.Resume();

            // Assert
            await oddsService.Received(1).GetOddsAsync(Arg.Any<string>(), matchId, 3, Arg.Any<string>());
            Assert.Single(viewModel.BetTypeOddsItems);
        }

        private Task FirstLoad(BetType betType)
        {
            oddsService.GetOddsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>())
                    .Returns(StubOdds());

            return viewModel.FirstLoadOrRefreshOddsAsync(betType, "decimal");
        }

        private static MatchOdds StubOdds() => StubOdds(BetType.AsianHDP.Value, BetType.AsianHDP.DisplayName);

        private static MatchOdds StubOdds(byte betTypeId, string betTypeName)
            => new MatchOdds
            (
                matchId,
                new List<BetTypeOdds>
                {
                    StubBetTypeOdds(betTypeId, betTypeName)
                }
            );

        private static BetTypeOdds StubBetTypeOdds(byte betTypeId, string bettypeName) => new BetTypeOdds
        (
            betTypeId,
            bettypeName,
            new Bookmaker("sr:book:1", "Bet188Com"),
            new List<BetOptionOdds>
            {
                new BetOptionOdds(  "home",  5.0m,  4.9m, "opt", "opening-opt", OddsTrend.Up ),
                new BetOptionOdds(  "draw",  3.2m,  3.2m, "opt", "opening-opt",  OddsTrend.Neutral ),
                new BetOptionOdds( "away",  2.5m,  2.8m, "opt", "opening-opt", OddsTrend.Down)
            }
        );

        private static BetTypeOdds StubBetTypeOdds(byte betTypeId, string bettypeName, decimal homeLiveOdds)
            => new BetTypeOdds
           (
               betTypeId,
               bettypeName,
               new Bookmaker("sr:book:1", "Bet188Com"),
               new List<BetOptionOdds>
               {
                    new BetOptionOdds(  "home",  homeLiveOdds,  4.9m, "opt", "opening-opt", OddsTrend.Up ),
                    new BetOptionOdds(  "draw",  3.2m,  3.2m, "opt", "opening-opt",  OddsTrend.Neutral ),
                    new BetOptionOdds( "away",  2.5m,  2.8m, "opt", "opening-opt", OddsTrend.Down)
               }
           );
    }
}