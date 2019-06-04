namespace LiveScore.Score.ViewModels
{
    using LiveScore.Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;

#pragma warning disable S2931 // Classes with "IDisposable" members should implement "IDisposable"

    public class MatchDetailViewModel : ViewModelBase
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);
        private readonly HubConnection matchHubConnection;
        private CancellationTokenSource cancellationTokenSource;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            IHubService hubService)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            matchHubConnection = hubService.BuildMatchHubConnection();
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDateAndLeagueName { get; set; }

        public string DisplayScore { get; set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            cancellationTokenSource = new CancellationTokenSource();

            if (parameters != null)
            {
                var match = parameters["Match"] as IMatch;
                MatchViewModel = new MatchViewModel(match, NavigationService, DepdendencyResolver, EventAggregator, matchHubConnection, true);
                BuildMatchDetailData(match);

                await StartListeningMatchHubEvent(match);
            }
        }

        protected override void Clean()
        {
            base.Clean();

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }

        private async Task StartListeningMatchHubEvent(IMatch match)
        {
            matchHubConnection.On<string, Dictionary<string, MatchPushEvent>>("PushMatchEvent", (sportId, payload) =>
            {
                if (sportId != SettingsService.CurrentSportType.Value || !payload.ContainsKey(match.Id))
                {
                    return;
                }

                var matchPayload = payload[match.Id];
                match.MatchResult = matchPayload.MatchResult;
                match.TimeLines = matchPayload.TimeLines;
                MatchViewModel.BuildMatchStatus();
                BuildMatchDetailData(match);
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

        private void BuildMatchDetailData(IMatch match)
        {
            var eventDate = match.EventDate.ToString("MMM dd, yyyy");
            DisplayEventDateAndLeagueName = $"{eventDate} - {match.League.Name.ToUpperInvariant()}";

            var homeScore = BuildScore(match.MatchResult.EventStatus, match.MatchResult.HomeScore);
            var awayScore = BuildScore(match.MatchResult.EventStatus, match.MatchResult.AwayScore);
            DisplayScore = $"{homeScore} - {awayScore}";
        }

        private static string BuildScore(MatchStatus matchStatus, int score)
        {
            return matchStatus.IsNotStarted ? string.Empty : score.ToString();
        }
    }

#pragma warning restore S2931 // Classes with "IDisposable" members should implement "IDisposable"
}