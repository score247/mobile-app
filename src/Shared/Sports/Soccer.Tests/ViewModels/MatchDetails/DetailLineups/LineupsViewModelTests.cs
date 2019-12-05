using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Teams;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsViewModelTests
    {
        private readonly ISoccerMatchService soccerMatchService;
        private readonly IDeviceInfo deviceInfo;
        private readonly string matchId = "match_id";
        private readonly Action<Action> beginInvokeOnMainThreadFunc;
        private readonly LineupsViewModel lineupsViewModel;
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly ISettings settings;
        private readonly Fixture fixture;

        public LineupsViewModelTests()
        {
            navigationService = Substitute.For<INavigationService>();
            serviceLocator = Substitute.For<IDependencyResolver>();
            eventAggregator = Substitute.For<IEventAggregator>();
            soccerMatchService = Substitute.For<ISoccerMatchService>();
            deviceInfo = Substitute.For<IDeviceInfo>();
            settings = Substitute.For<ISettings>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();
            networkConnectionManager.IsSuccessfulConnection().Returns(true);

            beginInvokeOnMainThreadFunc = ac => ac();

            serviceLocator.Resolve<ISoccerMatchService>().Returns(soccerMatchService);
            serviceLocator.Resolve<IDeviceInfo>().Returns(deviceInfo);
            serviceLocator.Resolve<Action<Action>>(FuncNameConstants.BeginInvokeOnMainThreadFuncName).Returns(beginInvokeOnMainThreadFunc);
            serviceLocator.Resolve<INetworkConnection>().Returns(networkConnectionManager);
            serviceLocator.Resolve<ISettings>().Returns(settings);
            settings.CurrentSportType.Returns(SportType.Soccer);
            fixture = new Fixture();

            lineupsViewModel = new LineupsViewModel(matchId, DateTime.Now, navigationService, serviceLocator, eventAggregator, new DataTemplate());
        }

        [Fact]
        public async Task LoadLineUpsAsync_LineupIsEmpty_SetHasDataToFalse()
        {
            // Arrange

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.False(lineupsViewModel.HasData);
        }

        [Fact]
        public async Task LoadLineUpsAsync_LineupIsEmpty_SetHasDataToTrue()
        {
            // Arrange
            var matchLineups = fixture.Create<MatchLineups>();
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.True(lineupsViewModel.HasData);
        }

        [Fact]
        public async Task RenderMatchLineups_HasFormation_SetHasFormationToTrue()
        {
            // Arrange
            var dumpTeamlineups = new TeamLineups(
                "sr:team:",
                "home",
                true,
                fixture.Create<Coach>(),
                "4-3-3",
                fixture.Create<List<Player>>(),
                fixture.Create<List<Player>>(),
                fixture.Create<List<TimelineEvent>>());

            var matchLineups = new MatchLineups(
                "match:lineups",
                DateTimeOffset.Now,
                dumpTeamlineups,
                dumpTeamlineups,
                ""
                );
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.True(lineupsViewModel.HasFormation);
        }

        [Fact]
        public async Task RenderMatchLineups_NoFormation_SetHasFormationToFalse()
        {
            // Arrange
            var dumpTeamlineups = new TeamLineups(
                "sr:team:",
                "home",
                true,
                fixture.Create<Coach>(),
                null,
                fixture.Create<List<Player>>(),
                fixture.Create<List<Player>>(),
                fixture.Create<List<TimelineEvent>>());

            var matchLineups = new MatchLineups(
                "match:lineups",
                DateTimeOffset.Now,
                dumpTeamlineups,
                dumpTeamlineups,
                ""
                );
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.False(lineupsViewModel.HasFormation);
        }

        [Fact]
        public async Task RenderMatchLineups_HasFormation_LineupsPitchIsNotNull()
        {
            // Arrange
            var dumpTeamlineups = new TeamLineups(
                "sr:team:",
                "home",
                true,
                fixture.Create<Coach>(),
                "4-3-3",
                fixture.Create<List<Player>>(),
                fixture.Create<List<Player>>(),
                fixture.Create<List<TimelineEvent>>());

            var matchLineups = new MatchLineups(
                "match:lineups",
                DateTimeOffset.Now,
                dumpTeamlineups,
                dumpTeamlineups,
                ""
                );
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.NotNull(lineupsViewModel.LineupsPitch);
        }

        [Fact]
        public async Task RenderMatchLineups_NoFormation_LineupsPitchIsNull()
        {
            // Arrange
            var dumpTeamlineups = new TeamLineups(
                "sr:team:",
                "home",
                true,
                fixture.Create<Coach>(),
                null,
                fixture.Create<List<Player>>(),
                fixture.Create<List<Player>>(),
                fixture.Create<List<TimelineEvent>>());

            var matchLineups = new MatchLineups(
                "match:lineups",
                DateTimeOffset.Now,
                dumpTeamlineups,
                dumpTeamlineups,
                ""
                );
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.Null(lineupsViewModel.LineupsPitch);
        }

        [Fact]
        public async Task RenderMatchLineups_HasFormation_LineupsItemGroupsDoesNotContainLineupsPlayerGroup()
        {
            // Arrange
            var dumpTeamlineups = new TeamLineups(
                "sr:team:",
                "home",
                true,
                fixture.Create<Coach>(),
                "4-3-3",
                fixture.Create<List<Player>>(),
                fixture.Create<List<Player>>(),
                fixture.Create<List<TimelineEvent>>());

            var matchLineups = new MatchLineups(
                "match:lineups",
                DateTimeOffset.Now,
                dumpTeamlineups,
                dumpTeamlineups,
                ""
                );
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.Null(lineupsViewModel.LineupsItemGroups.FirstOrDefault(group => group.Name == AppResources.LineupsPlayers));
        }

        [Fact]
        public async Task RenderMatchLineups_NoFormation_LineupsItemGroupsContainsLineupsPlayerGroup()
        {
            // Arrange
            var dumpTeamlineups = new TeamLineups(
                "sr:team:",
                "home",
                true,
                fixture.Create<Coach>(),
                null,
                fixture.Create<List<Player>>(),
                fixture.Create<List<Player>>(),
                fixture.Create<List<TimelineEvent>>());

            var matchLineups = new MatchLineups(
                "match:lineups",
                DateTimeOffset.Now,
                dumpTeamlineups,
                dumpTeamlineups,
                ""
                );
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now).Returns(Task.FromResult(matchLineups));

            // Act
            await lineupsViewModel.LoadLineUpsAsync();

            // Assert
            Assert.NotNull(lineupsViewModel.LineupsItemGroups.FirstOrDefault(group => group.Name == AppResources.LineupsPlayers));
        }
    }
}