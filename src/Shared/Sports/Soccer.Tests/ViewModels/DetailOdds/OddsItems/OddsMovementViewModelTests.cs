namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.PubSubEvents.Odds;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds;
    using NSubstitute;
    using Prism.Events;
    using Prism.Navigation;
    using Xunit;

    public class OddsMovementViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly OddsMovementViewModel viewModel;
        private readonly IOddsService oddsService;
        private readonly CompareLogic comparer;
        private readonly ILoggingService loggingService;

        private readonly IEventAggregator eventAggregator;

        private const string matchId = "sr:match:1";
        private readonly Bookmaker bookmaker;
        private readonly BetType betType = BetType.OneXTwo;
        private readonly string oddsFormat = "dec";

        public OddsMovementViewModelTests(ViewModelBaseFixture baseFixture)
        {
            bookmaker = new Bookmaker ( "sr:book:1", "Bet188" );

            comparer = baseFixture.CommonFixture.Comparer;
            loggingService = Substitute.For<ILoggingService>();
            oddsService = Substitute.For<IOddsService>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();

            baseFixture.DependencyResolver.Resolve<IOddsService>("1").Returns(oddsService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(loggingService);
            baseFixture.DependencyResolver.Resolve<IHubService>("1").Returns(baseFixture.HubService);
            baseFixture.DependencyResolver.Resolve<INetworkConnection>().Returns(networkConnectionManager);

            eventAggregator = Substitute.For<IEventAggregator>();
            eventAggregator.GetEvent<ConnectionChangePubSubEvent>().Returns(new ConnectionChangePubSubEvent());
            eventAggregator.GetEvent<OddsMovementPubSubEvent>().Returns(new OddsMovementPubSubEvent());


            viewModel = new OddsMovementViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                null);

            var parameters = new NavigationParameters
            {
                { "MatchId", matchId },
                { "Bookmaker", bookmaker},
                { "BetType", betType },
                { "Format",  oddsFormat}
            };

            viewModel.Initialize(parameters);
        }

        private MatchOddsMovement CreateMatchOddsMovement()
            => new MatchOddsMovement(
                matchId, 
                bookmaker, 
                new List<OddsMovement> { CreateOddsMovements() });

        private OddsMovement CreateOddsMovements() =>
            new OddsMovement("KO", 0, 0, true, CreateBetOptions(), DateTimeOffset.Now);

        private List<BetOptionOdds> CreateBetOptions() 
            => new List<BetOptionOdds>
            {
                new BetOptionOdds( "home", 5.000m, 4.900m, "", "", OddsTrend.Up ),
                new BetOptionOdds( "draw", 3.2m, 3.2m, "", "", OddsTrend.Neutral ),
                new BetOptionOdds( "away", 2.5m, 2.8m, "", "", OddsTrend.Down )
            };

        [Fact]
        public async Task FirstLoadOrRefreshOddsMovement_Always_GetOddsMovement() 
        {
            // Act
            await viewModel.FirstLoadOrRefreshOddsMovement();

            // Assert
            await oddsService.Received(1).GetOddsMovementAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>(), Arg.Any<string>());
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task FirstLoadOrRefreshOddsMovement_HasData_AssignedOddsMovementItems()
        {
            // Arrange
            oddsService.GetOddsMovementAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(CreateMatchOddsMovement());

            // Act
            await viewModel.FirstLoadOrRefreshOddsMovement();

            // Assert
            Assert.True(viewModel.HasData);
        }

        [Fact]
        public void OnAppearing_NoData()
        {
            // Arrange

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.False(viewModel.HasData);
            Assert.False(viewModel.IsRefreshing);
        }

        [Fact]
        public async Task RefreshCommand_OnExecute_LoadOddsMovement()
        {
            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await oddsService.Received(1).GetOddsMovementAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void HandleOddsMovementMessage_NotCurrentMatch_ShouldReturn()
        {
            // Arrange
            var oddsMovement = new OddsMovementMessage
            (
                1,
                "sr:amtch:2",
                new List<OddsMovementEvent> { StubOddsMovementEvent(BetType.OneXTwo.Value, bookmaker) }
            );

            // Act
            viewModel.HandleOddsMovementMessage(oddsMovement);

            // Assert
            Assert.Empty(viewModel.OddsMovementItems);
        }

        [Fact]
        public void HandleOddsMovementMessage_ForCurrentMatch_AddNew()
        {
            // Arrange
            var oddsMovement = new OddsMovementMessage
            (
                1,
                matchId,
                new List<OddsMovementEvent> { StubOddsMovementEvent(BetType.OneXTwo.Value, bookmaker) }
            );

            // Act
            viewModel.HandleOddsMovementMessage(oddsMovement);

            // Assert
            Assert.NotEmpty(viewModel.OddsMovementItems);
            Assert.Single(viewModel.OddsMovementItems);
        }

        private static OddsMovementEvent StubOddsMovementEvent(byte betTypeId, Bookmaker bookmaker)
            => new OddsMovementEvent(
                betTypeId, 
                bookmaker, 
                new OddsMovement("Live", 0, 0, true, new List<BetOptionOdds>
                {
                    new BetOptionOdds( "home", 5.200m, 4.900m, "", "", OddsTrend.Up ),
                    new BetOptionOdds( "draw", 3.1m, 3.2m, "", "", OddsTrend.Down ),
                    new BetOptionOdds( "away", 2.4m, 2.8m, "", "", OddsTrend.Down )
                }, DateTimeOffset.Now));
    }
}
