namespace Soccer.Tests.ViewModels.DetailOdds
{
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Newtonsoft.Json;
    using NSubstitute;
    using Prism.Navigation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class OddsMovementViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private const string matchId = "sr:match:1";
        private const string format = "dec";
        private static readonly BetType betType = BetType.OneXTwo;     

        private readonly OddsMovementViewModel viewModel;
        private readonly IOddsService oddsService;
        private readonly CompareLogic comparer;
        private readonly ILoggingService mockLogService;
        private readonly INavigationService navigationService;

        private readonly Bookmaker bookmaker = new Bookmaker { Id = "sr:book:1", Name = "Unibet" };

        public OddsMovementViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            mockLogService = Substitute.For<ILoggingService>();
            oddsService = Substitute.For<IOddsService>();

            navigationService = baseFixture.NavigationService;

            baseFixture.DependencyResolver.Resolve<IOddsService>("1").Returns(oddsService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(mockLogService);

            viewModel = new OddsMovementViewModel(
                navigationService,
                baseFixture.DependencyResolver,
                null);

            var parameters = new NavigationParameters {
                { "MatchId", matchId },
                { "Bookmaker", bookmaker },
                { "BetType", betType },
                { "Format", format },
            };

            viewModel.OnNavigatingTo(parameters);
        }

        [Fact]
        public void OnNavigatingTo_Title_ShouldBeCorrectFormat()
        {
            Assert.Equal("Unibet - 1X2 Odds", viewModel.Title);
        }

        [Fact]
        public void OnNavigatingTo_Error_ShouldLogError()
        {
            var parameters = new NavigationParameters {
                { "MatchId", matchId },
                { "Bookmaker", null},
                { "BetType", BetType.OneXTwo },
                { "Format", "dec" },
            };

            viewModel.OnNavigatingTo(parameters);

            mockLogService.Received(1).LogError(Arg.Any<Exception>());
        }

        [Fact]
        public void OnAppearing_Always_GetOddsMovement()
        {
            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);

            oddsService.Received(1).GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id);
        }

        [Fact]
        public void OnAppearing_GetOddsMovement_CorrectAssignedItems()
        {
            // Arrange
            oddsService
                .GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id)
                .Returns(CreateMatchOddsMovement());

            var expectedViewModels = new List<BaseMovementItemViewModel>
            {
                new BaseMovementItemViewModel(betType, CreateOddsMovement(true), viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseMovementItemViewModel(betType, CreateOddsMovement(), viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.IsNotLoading);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
            Assert.Equal(2, viewModel.OddsMovementItems.Count);
            Assert.True(comparer.Compare(expectedViewModels, viewModel.OddsMovementItems).AreEqual);
        }

        [Fact]
        public async Task RefreshCommand_OnExecute_LoadOdds()
        {
            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await oddsService.Received(1).GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id, true);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
        }

        public MatchOddsMovement CreateMatchOddsMovement()
            => new MatchOddsMovement
            {
                MatchId = matchId,
                Bookmaker = bookmaker,
                OddsMovements = new List<OddsMovement>
                {
                    CreateOddsMovement(true),
                    CreateOddsMovement()                    
                }
            };

        public OddsMovement CreateOddsMovement(bool isMatchStarted = false)
        {
            if (isMatchStarted)
            {
                return new OddsMovement
                {
                    HomeScore = 1,
                    AwayScore = 0,
                    MatchTime = "5'",                   
                    IsMatchStarted = isMatchStarted,
                    UpdateTime = new DateTime(2019, 8, 26),
                    BetOptions = CreateBetOptions()
                };
            }
            else
            {
                return new OddsMovement
                {                    
                    IsMatchStarted = isMatchStarted,
                    UpdateTime = new DateTime(2019, 8, 26),
                    BetOptions = CreateBetOptions()
                };
            }
        }

        public List<BetOptionOdds> CreateBetOptions()
        => new List<BetOptionOdds>
            {
                new BetOptionOdds{ Type = "home", LiveOdds = 5.0m, OpeningOdds = 4.9m, OddsTrend = OddsTrend.Up },
                new BetOptionOdds{ Type = "draw", LiveOdds = 3.2m, OpeningOdds = 3.2m, OddsTrend = OddsTrend.Neutral },
                new BetOptionOdds{ Type = "away", LiveOdds = 2.5m, OpeningOdds = 2.8m, OddsTrend = OddsTrend.Down }
            };

        [Fact]
        public async Task DeserializeOddsMovementMessage_Null_ShouldLogError()
        {
            // Act
            await viewModel.DeserializeOddsMovementMessage(null);

            // Assert
            await mockLogService.Received(1).LogErrorAsync(Arg.Any<string>(), Arg.Any<Exception>());
        }

        [Fact]
        public async Task DeserializeOddsMovementMessage_NotNull_CorrectAssignedItem()
        {
            // Arrange
            var message = CreateMatchOddsMovementMessage();
            var jsonMessage = JsonConvert.SerializeObject(message);

            // Act
            var actual = await viewModel.DeserializeOddsMovementMessage(jsonMessage);

            // Assert
            Assert.True(comparer.Compare(message, actual).AreEqual);
        }

        [Fact]
        public async Task HandleOddsMovementMessage_EmptyMovementEvents_NotUpdateOddsMovement()
        {
            // Arrange
            var message = CreateMatchOddsMovementEmptyEventMessage();

            // Act
            await viewModel.HandleOddsMovementMessage(message);

            // Assert
            await oddsService.DidNotReceive().GetOddsMovement(Arg.Any<string>(), matchId, Arg.Any<byte>(), format, bookmaker.Id);
        }

        [Fact]
        public async Task HandleOddsMovementMessage_HasMovementEvents_ShouldAddNew()
        {
            // Arrange
            var message = CreateMatchOddsMovementMessage();

            oddsService
                .GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id)
                .Returns(CreateMatchOddsMovement());

            viewModel.OnAppearing();

            // Act
            await viewModel.HandleOddsMovementMessage(message);

            // Assert
            await oddsService.Received(1).GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id);
            Assert.Equal(3, viewModel.OddsMovementItems.Count);
        }

        private MatchOddsMovementMessage CreateMatchOddsMovementEmptyEventMessage() 
            => new MatchOddsMovementMessage
            {
                MatchId = matchId,
                OddsEvents = new List<OddsMovementEvent> { }
            };

        private MatchOddsMovementMessage CreateMatchOddsMovementMessage()
           => new MatchOddsMovementMessage
           {
               MatchId = matchId,
               OddsEvents = new List<OddsMovementEvent>
               {
                   new OddsMovementEvent(BetType.OneXTwo.Value, bookmaker, CreateOddsMovement())
               }
           };

        [Fact]
        public async Task OnResume_Always_LoadOdds()
        {
            // Arrange 
            oddsService
               .GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id, true)
               .Returns(CreateMatchOddsMovement());

            // Act            
            viewModel.OnResume();

            // Assert            
            await oddsService.Received(1).GetOddsMovement(Arg.Any<string>(), matchId, betType.Value, format, bookmaker.Id, true);
            Assert.Equal(2, viewModel.OddsMovementItems.Count);
        }
    }
}
