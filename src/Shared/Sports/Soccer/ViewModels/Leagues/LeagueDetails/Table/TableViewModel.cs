using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
using LiveScore.Core.Services;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Teams;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModel : TabItemViewModel, IDisposable
    {
        private readonly LeagueDetailNavigationParameter currentLeague;
        private readonly ILeagueService leagueService;
        private readonly bool highlightTeamName;
        private bool disposed;

#pragma warning disable S107 // Methods should not have too many parameters

        public TableViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            LeagueDetailNavigationParameter league,
            DataTemplate dataTemplate = null,
            string countryFlag = null,
            string homeTeamId = null,
            string awayTeamId = null,
            bool highlightTeamName = false)
                : base(navigationService, serviceLocator, dataTemplate, null, AppResources.Table)
        {
            IsBusy = true;
            currentLeague = league;
            CurrentLeagueName = league.LeagueGroupName?.ToUpperInvariant();
            CurrentCountryFlag = countryFlag;
            CurrentHomeTeamId = homeTeamId;
            CurrentAwayTeamId = awayTeamId;
            this.highlightTeamName = highlightTeamName;
            IsActive = true;

            leagueService = DependencyResolver.Resolve<ILeagueService>(SportType.Soccer.Value.ToString());
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
        }

#pragma warning restore S107 // Methods should not have too many parameters

        public IReadOnlyList<TeamStanding> TeamStandingsItemSource { get; private set; }

        public IReadOnlyList<LeagueGroupNote> GroupNotesItemSource { get; private set; }

        public IReadOnlyList<TeamOutcome> OutcomesItemSource { get; private set; }

        public string CurrentCountryFlag { get; }

        public string CurrentLeagueName { get; }

        public string CurrentHomeTeamId { get; }

        public string CurrentAwayTeamId { get; }

        public bool IsRefreshing { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public bool VisibleTableHeader { get; private set; }

        public override async void OnResumeWhenNetworkOK()
            => await LoadDataAsync(LoadLeagueTableAsync);

        public override Task OnNetworkReconnectedAsync()
            => LoadDataAsync(LoadLeagueTableAsync);

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadLeagueTableAsync);
        }

        public override void Destroy()
        {
            base.Destroy();

            Dispose();
        }

        private async Task OnRefresh()
        {
            await LoadDataAsync(LoadLeagueTableAsync, false);
            IsRefreshing = false;
        }

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        internal async Task LoadLeagueTableAsync()
        {
            if (!(await leagueService.GetTable(
                currentLeague.Id,
                currentLeague.SeasonId,
                currentLeague.RoundGroup,
                CurrentLanguage) is LeagueTable leagueTable) || leagueTable.GroupTables?.Any() != true)
            {
                HasData = false;
                return;
            }

            var table = leagueTable.GroupTables.FirstOrDefault();

            if (table == null || TableHasNoDataForCurrentTeams(table))
            {
                HasData = false;
                return;
            }

            BuildLeagueTable(table);
        }

        private void BuildLeagueTable(LeagueGroupTable table)
        {
            BuildTeamStandings(table);
            BuildOutcomes(table);
            GroupNotesItemSource = table.GroupNotes?.ToList();
            VisibleTableHeader = true;
            HasData = true;
        }

        private void BuildTeamStandings(LeagueGroupTable table)
        {
            var teamStandings = table.TeamStandings.OrderBy(standing => standing.Rank).ToList();

            foreach (var teamStanding in teamStandings)
            {
                if (highlightTeamName && (teamStanding.Id == CurrentHomeTeamId || teamStanding.Id == CurrentAwayTeamId))
                {
                    teamStanding.IsHightLight = true;
                }

                teamStanding.Outcome.ColorResourceKey = Enumeration.FromValue<TeamOutcome>(teamStanding.Outcome.Value).ColorResourceKey;
            }

            TeamStandingsItemSource = teamStandings;
        }

        private void BuildOutcomes(LeagueGroupTable table)
        {
            if (table.OutcomeList == null)
            {
                return;
            }

            foreach (var outcome in table.OutcomeList)
            {
                outcome.ColorResourceKey = Enumeration.FromValue<TeamOutcome>(outcome.Value).ColorResourceKey;
            }

            OutcomesItemSource = table.OutcomeList.ToList();
        }

        private bool TableHasNoDataForCurrentTeams(LeagueGroupTable table)
            => CurrentHomeTeamId != null
                && !table.TeamStandings.Any(team => team.Id == CurrentHomeTeamId || team.Id == CurrentAwayTeamId);

#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                TeamStandingsItemSource = null;
                GroupNotesItemSource = null;
                OutcomesItemSource = null;
            }

            disposed = true;
        }
    }
}