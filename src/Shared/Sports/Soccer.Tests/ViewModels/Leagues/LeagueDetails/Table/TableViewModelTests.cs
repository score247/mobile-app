using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using NSubstitute;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private const string CurrentLeagueId = "league:1";
        private const string CurrentLeagueSeasonId = "leagueSeason:1";
        private const string CurrentLeagueRoundGroup = "A";
        private const string CurrentHomeId = "team:1";
        private const string CurrentAwayId = "team:2";
        private const string CurrentLeagueName = "leagueName";
        private const string CurrentCountryFlag = "vietnam";
        private readonly DataTemplate subDataTemplate;
        private readonly ILeagueService leagueService;
        private readonly ViewModelBaseFixture baseFixture;
        private readonly Fixture specimens;
        private readonly TableViewModel viewModel;

        public TableViewModelTests(ViewModelBaseFixture baseFixture)
        {
            specimens = new Fixture();
            this.baseFixture = baseFixture;
            leagueService = Substitute.For<ILeagueService>();
            subDataTemplate = Substitute.For<DataTemplate>();
            this.baseFixture.DependencyResolver
                .Resolve<ILeagueService>("1")
                .Returns(leagueService);
            baseFixture.NetworkConnection.IsSuccessfulConnection().Returns(true);

            viewModel = new TableViewModel(
                this.baseFixture.NavigationService,
                this.baseFixture.DependencyResolver,
                new LeagueDetailNavigationParameter(CurrentLeagueId, CurrentLeagueName, 0, "", false, CurrentLeagueRoundGroup, CurrentLeagueSeasonId, true),
                subDataTemplate,
                countryFlag: CurrentCountryFlag,
                homeTeamId: CurrentHomeId,
                awayTeamId: CurrentAwayId,
                highlightTeamName: true);
        }

        [Fact]
        public void Constructor_Always_GetExpectedAssignedProps()
        {
            // Assert
            Assert.Equal(CurrentLeagueName.ToUpperInvariant(), viewModel.CurrentLeagueName);
            Assert.Equal(CurrentCountryFlag, viewModel.CurrentCountryFlag);
            Assert.Equal(CurrentHomeId, viewModel.CurrentHomeTeamId);
            Assert.Equal(CurrentAwayId, viewModel.CurrentAwayTeamId);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task OnAppearing_Always_CallServiceToGetData()
        {
            // Arrange
            viewModel.OnAppearing();
            await Task.Delay(100);

            // Assert
            await leagueService
                .Received(1)
                .GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage);
        }

        [Fact]
        public async Task OnResumeWhenNetworkOK_Always_CallServiceToGetData()
        {
            // Arrange
            viewModel.OnResumeWhenNetworkOK();
            await Task.Delay(100);

            // Assert
            await leagueService
                .Received(1)
                .GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage);
        }

        [Fact]
        public async Task OnNetworkReconnectedAsync_Always_CallServiceToGetData()
        {
            // Arrange
            await viewModel.OnNetworkReconnectedAsync();
            await Task.Delay(100);

            // Assert
            await leagueService
                .Received(1)
                .GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage);
        }

        [Fact]
        public async Task OnRefresh_Always_CallServiceToGetData()
        {
            // Arrange
            await viewModel.RefreshCommand.ExecuteAsync();
            await Task.Delay(100);

            // Assert
            await leagueService
                .Received(1)
                .GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage);
            Assert.False(viewModel.IsRefreshing);
        }

#pragma warning restore S2699 // Tests should include assertions

        [Fact]
        public async Task LoadLeagueTableAsync_GetTableHasNoData_ShowNoData()
        {
            // Arrange
            var emptyLeagueTable = new LeagueTable(null, null, null, null);
            leagueService.GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
               .Returns(emptyLeagueTable);

            // Act
            await viewModel.LoadLeagueTableAsync();

            // Assert
            Assert.False(viewModel.HasData);
            Assert.False(viewModel.VisibleTableHeader);
            Assert.Null(viewModel.TeamStandingsItemSource);
            Assert.Null(viewModel.GroupNotesItemSource);
            Assert.Null(viewModel.OutcomesItemSource);
        }

        [Fact]
        public async Task LoadLeagueTableAsync_GetTableHasData_ShowExpectedTeamStandings()
        {
            // Arrange
            var teamStandings = BuildTeamStandings();
            var leagueTable = BuildLeagueTable(teamStandings);

            leagueService.GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                   .Returns(leagueTable);

            // Act
            await viewModel.LoadLeagueTableAsync();

            // Assert
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.VisibleTableHeader);
            Assert.Equal(teamStandings.OrderBy(s => s.Rank), viewModel.TeamStandingsItemSource);
        }

        [Fact]
        public async Task LoadLeagueTableAsync_StandingHasHomeTeamOrwAwayTeamId_SetHighlight()
        {
            // Arrange
            var teamStandings = BuildTeamStandings();
            var leagueTable = BuildLeagueTable(teamStandings);
            leagueService.GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                   .Returns(leagueTable);

            // Act
            await viewModel.LoadLeagueTableAsync();

            // Assert
            var expectedHomeStanding = viewModel.TeamStandingsItemSource.First(s => s.Id == CurrentHomeId);
            var expectedAwayStanding = viewModel.TeamStandingsItemSource.First(s => s.Id == CurrentAwayId);
            Assert.True(expectedHomeStanding.IsHightLight);
            Assert.True(expectedAwayStanding.IsHightLight);
        }

        [Fact]
        public async Task LoadLeagueTableAsync_HasGroupNotes_GetExpectedGroupNotes()
        {
            // Arrange
            var teamStandings = BuildTeamStandings();
            var groupNotes = BuildLeagueGroupNotes();
            var leagueTable = BuildLeagueTable(teamStandings, groupNotes);
            leagueService.GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                   .Returns(leagueTable);

            // Act
            await viewModel.LoadLeagueTableAsync();

            // Assert
            Assert.Equal(groupNotes, viewModel.GroupNotesItemSource);
        }

        [Fact]
        public async Task LoadLeagueTableAsync_HasOutcomes_GetExpectedOutcomes()
        {
            // Arrange
            var teamStandings = BuildTeamStandings();
            var outcomes = BuildLeagueOutcomes();
            var leagueTable = BuildLeagueTable(teamStandings, teamOutcomes: outcomes);
            leagueService.GetTable(CurrentLeagueId, CurrentLeagueSeasonId, CurrentLeagueRoundGroup, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                   .Returns(leagueTable);

            // Act
            await viewModel.LoadLeagueTableAsync();

            // Assert
            Assert.Equal(outcomes, viewModel.OutcomesItemSource);
        }

        private List<TeamStanding> BuildTeamStandings() => new List<TeamStanding> {
            specimens.Create<TeamStanding>()
                .With(standing => standing.Rank, 1)
                .With(standing => standing.Outcome, TeamOutcome.AFCChampionsLeague)
                .With(standing => standing.Id, CurrentHomeId),
            specimens.Create<TeamStanding>()
                .With(standing => standing.Rank, 3)
                .With(standing => standing.Outcome, TeamOutcome.AFCChampionsLeague)
                .With(standing => standing.Id, "team:3"),
            specimens.Create<TeamStanding>()
                .With(standing => standing.Rank, 4)
                .With(standing => standing.Outcome, TeamOutcome.AFCChampionsLeague)
                .With(standing => standing.Id, "team:4"),
             specimens.Create<TeamStanding>()
                .With(standing => standing.Rank, 2)
                .With(standing => standing.Outcome, TeamOutcome.AFCChampionsLeague)
                .With(standing => standing.Id, CurrentAwayId)
        };

        private IEnumerable<LeagueGroupNote> BuildLeagueGroupNotes() => specimens.CreateMany<LeagueGroupNote>();

        private IEnumerable<TeamOutcome> BuildLeagueOutcomes() => new List<TeamOutcome>
        {
            TeamOutcome.AFCChampionsLeague,
            TeamOutcome.AFCCup
        };

        private LeagueTable BuildLeagueTable(List<TeamStanding> teamStandings, IEnumerable<LeagueGroupNote> leagueGroupNotes = null, IEnumerable<TeamOutcome> teamOutcomes = null)
        {
            return specimens.Create<LeagueTable>()
                .With(table => table.GroupTables, new List<LeagueGroupTable> {
                    specimens.Create<LeagueGroupTable>()
                        .With(t => t.TeamStandings, teamStandings)
                        .With(t => t.OutcomeList, teamOutcomes)
                        .With(t => t.GroupNotes, leagueGroupNotes)
                });
        }
    }
}