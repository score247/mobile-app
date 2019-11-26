﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Teams;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModel : TabItemViewModel
    {
        private readonly string currentLeagueId;
        private readonly string currentLeagueSeasonId;
        private readonly string currentLeagueRoundGroup;
        private readonly ILeagueService leagueService;

#pragma warning disable S107 // Methods should not have too many parameters

        public TableViewModel(
            string leagueId,
            string leagueSeasonId,
            string leagueRoundGroup,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate = null,
            string leagueName = null,
            string countryFlag = null,
            string homeTeamId = null,
            string awayTeamId = null)
            : base(navigationService, serviceLocator, dataTemplate, null, AppResources.Table)
        {
            currentLeagueId = leagueId;
            currentLeagueSeasonId = leagueSeasonId;
            currentLeagueRoundGroup = leagueRoundGroup;
            CurrentLeagueName = leagueName;
            CurrentLeagueFlag = countryFlag;
            CurrentHomeTeamId = homeTeamId;
            CurrentAwayTeamId = awayTeamId;
            leagueService = DependencyResolver.Resolve<ILeagueService>(SportType.Soccer.Value.ToString());
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
        }

#pragma warning restore S107 // Methods should not have too many parameters

        public IReadOnlyList<TeamStanding> TeamStandingsItemSource { get; private set; }

        public IReadOnlyList<LeagueGroupNote> GroupNotesItemSource { get; private set; }

        public IReadOnlyList<TeamOutcome> OutcomesItemSource { get; private set; }

        public string CurrentLeagueFlag { get; }

        public string CurrentLeagueName { get; }

        public string CurrentHomeTeamId { get; }

        public string CurrentAwayTeamId { get; }

        public bool IsRefreshing { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public override async void OnResumeWhenNetworkOK()
            => await LoadDataAsync(LoadLeagueTableAsync);

        public override Task OnNetworkReconnectedAsync()
            => LoadDataAsync(LoadLeagueTableAsync);

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadLeagueTableAsync);
        }

        private Task OnRefresh()
            => LoadDataAsync(LoadLeagueTableAsync, false);

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        private async Task LoadLeagueTableAsync()
        {
            var leagueTable = await leagueService.GetTable(
                currentLeagueId,
                currentLeagueSeasonId,
                currentLeagueRoundGroup,
                CurrentLanguage) as LeagueTable;

            if (leagueTable == null || leagueTable.GroupTables?.Any() != true)
            {
                HasData = false;
                IsRefreshing = false;
                return;
            }

            var table = leagueTable.GroupTables.FirstOrDefault();
            var teamStandings = table.TeamStandings.OrderBy(standing => standing.Rank);

            foreach (var teamStanding in teamStandings)
            {
                if (teamStanding.Id == CurrentHomeTeamId || teamStanding.Id == CurrentAwayTeamId)
                {
                    teamStanding.IsHightLight = true;
                }

                teamStanding.Outcome.Color = Enumeration.FromValue<TeamOutcome>(teamStanding.Outcome.Value).Color;
            }

            TeamStandingsItemSource = teamStandings.ToList();
            GroupNotesItemSource = table.GroupNotes.ToList();

            foreach (var outcome in table.OutcomeList)
            {
                outcome.Color = Enumeration.FromValue<TeamOutcome>(outcome.Value).Color;
            }

            OutcomesItemSource = table.OutcomeList.ToList();

            HasData = true;
            IsRefreshing = false;
        }

#pragma warning restore S3215 // "interface" instances should not be cast to concrete types
    }
}