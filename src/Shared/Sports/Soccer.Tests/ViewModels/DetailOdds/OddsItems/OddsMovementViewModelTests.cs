namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using NSubstitute;
    using Prism.Navigation;
    using Xunit;

    public class OddsMovementViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly OddsMovementViewModel viewModel;
        private readonly IOddsService oddsService;
        private readonly CompareLogic comparer;
        private readonly ILoggingService loggingService;

        private const string matchId = "sr:match:1";
        private readonly Bookmaker bookmaker;
        private const BetType betType = BetType.OneXTwo;
        private const string oddsFormat = "dec";

        public OddsMovementViewModelTests(ViewModelBaseFixture baseFixture)
        {
            bookmaker = new Bookmaker { Id = "sr:book:1", Name = "Bet188" };

            comparer = baseFixture.CommonFixture.Comparer;
            loggingService = Substitute.For<ILoggingService>();
            oddsService = Substitute.For<IOddsService>();

            baseFixture.DependencyResolver.Resolve<IOddsService>("1").Returns(oddsService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>("1").Returns(loggingService);

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

            viewModel.OnNavigatingTo(parameters);
        }

        private MatchOddsMovement CreateMatchOddsMovement()
            => new MatchOddsMovement
            {
                OddsMovements = new List<OddsMovement> { CreateOddsMovements() }
            };

        private OddsMovement CreateOddsMovements() =>
            new OddsMovement
            {
                MatchTime = "KO",
                BetOptions = CreateBetOptions()
            };

        private List<BetOptionOdds> CreateBetOptions() => new List<BetOptionOdds>
            {
                new BetOptionOdds{ Type = "home", LiveOdds = 5.0m, OpeningOdds = 4.9m, OddsTrend = OddsTrend.Up },
                new BetOptionOdds{ Type = "draw", LiveOdds = 3.2m, OpeningOdds = 3.2m, OddsTrend = OddsTrend.Neutral },
                new BetOptionOdds{ Type = "away", LiveOdds = 2.5m, OpeningOdds = 2.8m, OddsTrend = OddsTrend.Down }
            };

        [Fact]
        public void OnAppearing_Always_LoadOddsMovement()
        {
            // Arrange
            oddsService.GetOddsMovement(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(CreateMatchOddsMovement());
            var expectedViewModels = new ObservableCollection<BaseMovementItemViewModel>
            {
                new BaseMovementItemViewModel(BetType.OneXTwo, CreateOddsMovements(), viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.IsNotLoading);
            Assert.False(viewModel.IsRefreshing);
            Assert.False(viewModel.IsLoading);
            Assert.True(comparer.Compare(expectedViewModels, viewModel.OddsMovementItems).AreEqual);
        }

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
        public async Task RefreshCommand_OnExecute_LoadOddsMovement()
        {
            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await oddsService.Received(1).GetOddsMovement(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), true);
        }
    }
}
