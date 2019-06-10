﻿namespace LiveScore.Soccer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;

#pragma warning disable S2931 // Classes with "IDisposable" members should implement "IDisposable"

    public class MatchDetailViewModel : ViewModelBase
    {
        private const string SpectatorNumberFormat = "0,0";
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);
        private readonly HubConnection matchHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            IHubService hubService)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            matchHubConnection = hubService.BuildMatchHubConnection();
            matchService = DependencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.Value);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadMatchDetail(MatchViewModel.Match.Id, false, true));
        }

        public DelegateAsyncCommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; }

        public bool IsLoading { get; set; }

        public bool IsNotLoading { get; set; }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDateAndLeagueName { get; set; }

        public string DisplayScore { get; set; }

        public string DisplayEventDate { get; set; }

        public string DisplayAttendance { get; set; }

        public ObservableCollection<BaseInfoItemViewModel> InfoItemViewModels { get; set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                var match = parameters["Match"] as IMatch;
                MatchViewModel = new MatchViewModel(
                    match, NavigationService, DependencyResolver, EventAggregator, matchHubConnection, true);
                BuildData(match);
            }
        }

        public override async void OnAppearing()
        {
            await LoadMatchDetail(MatchViewModel.Match.Id);
            cancellationTokenSource = new CancellationTokenSource();

            await StartListeningMatchHubEvent();
        }

        protected override void Clean()
        {
            base.Clean();

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Dispose();
            }
        }

        private async Task LoadMatchDetail(string matchId, bool showLoadingIndicator = true, bool isRefresh = false)
        {
            IsLoading = showLoadingIndicator;

            var match = await matchService.GetMatch(SettingsService.UserSettings, matchId, isRefresh);
            BuildData(match);

            IsLoading = false;
            IsNotLoading = true;
            IsRefreshing = false;
        }

        private async Task StartListeningMatchHubEvent()
        {
            var match = MatchViewModel.Match;

            matchHubConnection.On<string, Dictionary<string, MatchPushEvent>>("PushMatchEvent", (sportId, payload) =>
            {
                if (sportId != SettingsService.CurrentSportType.Value || !payload.ContainsKey(match.Id))
                {
                    return;
                }

                var matchPayload = payload[match.Id];
                match.MatchResult = matchPayload.MatchResult;
                match.TimeLines = match.TimeLines.Concat(matchPayload.TimeLines);

                BuildData(match);
            });

            try
            {
                await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private void BuildData(IMatch match)
        {
            MatchViewModel = new MatchViewModel(match, NavigationService, DependencyResolver, EventAggregator, matchHubConnection, true);
            MatchViewModel.BuildMatchStatus();

            BuildInfoItems(match);
            BuildGeneralInfo(match);
            BuildFooterInfo(match);
        }

        private void BuildInfoItems(IMatch match)
        {
            var timelines = match?.TimeLines?
                 .Where(t => BaseInfoItemViewModel.InfoItemEventTypes.Contains(t.Type))
                 .OrderBy(t => t.Time).ToList() ?? new List<ITimeline>();

            InfoItemViewModels = new ObservableCollection<BaseInfoItemViewModel>(timelines.Select(t =>
                   new BaseInfoItemViewModel(t, match.MatchResult, NavigationService, DependencyResolver)
                   .CreateInstance()));
        }

        private void BuildGeneralInfo(IMatch match)
        {
            var eventDate = match.EventDate.ToDayMonthYear();
            DisplayEventDateAndLeagueName = $"{eventDate} - {match.League.Name.ToUpperInvariant()}";

            var homeScore = BuildScore(match.MatchResult.EventStatus, match.MatchResult.HomeScore);
            var awayScore = BuildScore(match.MatchResult.EventStatus, match.MatchResult.AwayScore);
            DisplayScore = $"{homeScore} - {awayScore}";
        }

        private void BuildFooterInfo(IMatch match)
        {
            DisplayEventDate = match.EventDate.ToFullDateTime();

            if (match.Attendance > 0)
            {
                DisplayAttendance = match.Attendance.ToString(SpectatorNumberFormat);
            }
        }

        private static string BuildScore(MatchStatus matchStatus, int score)
        {
            return matchStatus.IsPreMatch ? string.Empty : score.ToString();
        }
    }

#pragma warning restore S2931 // Classes with "IDisposable" members should implement "IDisposable"
}