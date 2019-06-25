using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.ViewModels.DetailOdds;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using LiveScore.Soccer.Views.Templates;
    using LiveScore.Soccer.Views.Templates.MatchDetailInfo;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MatchDetailViewModel : ViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private static Dictionary<string, ContentView> TabLayouts => new Dictionary<string, ContentView>
        {
            {"Odds", new OddsTemplate()},
            {"Info", new InfoTemplate()},
            {"H2H", new H2HTemplate()},
            {"Lineups", new LineupsTemplate()},
            {"Social", new SocialTemplate()},
            {"Stats", new StatsTemplate()},
            {"Table", new TableTemplate()},
            {"TV", new TVTemplate()},
            {"Tracker", new TrackerTemplate()},
        };

        private readonly HubConnection matchHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;
        private bool disposedValue;
        private Dictionary<string, ViewModelBase> tabViewModels;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            IHubService hubService)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            matchHubConnection = hubService.BuildMatchHubConnection();
            matchService = DependencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.Value);
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDateAndLeagueName { get; private set; }

        public string DisplayScore { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public string DisplayPenaltyShootOut { get; private set; }

        public ObservableCollection<BaseItemViewModel> InfoItemViewModels { get; private set; }

        public ObservableCollection<TabModel> TabViews { get; set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                BuildGeneralInfo(match);

                tabViewModels = new Dictionary<string, ViewModelBase>
                {
                    {"Odds", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"Info", new DetailInfoViewModel(match.Id, NavigationService, DependencyResolver, matchHubConnection)},
                    {"H2H", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"Lineups", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"Social", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"Stats", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"Table", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"TV", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                    {"Tracker", new DetailOddsViewModel(NavigationService, DependencyResolver)},
                };
            }
        }

        protected override void Clean()
        {
            base.Clean();

            cancellationTokenSource?.Dispose();
        }

        protected override async void Initialize()
        {
            try
            {
                await LoadMatchDetail(MatchViewModel.Match.Id);

                cancellationTokenSource = new CancellationTokenSource();

                await StartListeningMatchHubEvent();
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task LoadMatchDetail(string matchId)
        {
            var match = await matchService.GetMatch(SettingsService.UserSettings, matchId);

            BuildTabFunctions(match);
        }

        private void BuildTabFunctions(IMatch match)
        {
            if (match.MatchFunctions != null)
            {
                TabViews = new ObservableCollection<TabModel>();

                foreach (var tab in match.MatchFunctions)
                {
                    TabViews.Add(new TabModel
                    {
                        Name = tab.Abbreviation,
                        ContentTemplate = TabLayouts[tab.Abbreviation.Replace("-", string.Empty)],
                        ViewModel = tabViewModels[tab.Abbreviation.Replace("-", string.Empty)]
                    });
                }
            }
        }

        private async Task StartListeningMatchHubEvent()
        {
            matchHubConnection.On<string, Dictionary<string, MatchPushEvent>>("PushMatchEvent", OnReceivingMatchEvent);

            await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, cancellationTokenSource.Token);
        }

        protected internal void OnReceivingMatchEvent(string sportId, Dictionary<string, MatchPushEvent> payload)
        {
            var match = MatchViewModel.Match;

            if (sportId != SettingsService.CurrentSportType.Value || match?.Id == null || !payload.ContainsKey(match.Id))
            {
                return;
            }

            var matchPayload = payload[match.Id];
            match.MatchResult = matchPayload.MatchResult;
            match.LatestTimeline = matchPayload.TimeLines.LastOrDefault();

            BuildGeneralInfo(match);
        }

        private void BuildGeneralInfo(IMatch match)
        {
            BuildViewModel(match);

            BuildScoreAndEventDate(match);

            BuildSecondLeg(match);

            BuildPenaltyShootOut(match);
        }

        private void BuildPenaltyShootOut(IMatch match)
        {
            var penaltyResult = match.MatchResult?.GetPenaltyResult();

            if (penaltyResult != null && match.MatchResult.EventStatus.IsClosed)
            {
                DisplayPenaltyShootOut = $"{AppResources.PenaltyShootOut}: {penaltyResult.HomeScore} - {penaltyResult.AwayScore}";
            }
        }

        private void BuildSecondLeg(IMatch match)
        {
            var winnerId = match.MatchResult?.AggregateWinnerId;

            if (!string.IsNullOrEmpty(winnerId) && match.MatchResult.EventStatus.IsClosed)
            {
                DisplaySecondLeg = $"{AppResources.SecondLeg} {match.MatchResult.AggregateHomeScore} - {match.MatchResult.AggregateAwayScore}";
            }
        }

        private void BuildScoreAndEventDate(IMatch match)
        {
            var eventDate = match.EventDate.ToDayMonthYear();
            DisplayEventDateAndLeagueName = $"{eventDate} - {match.League?.Name?.ToUpperInvariant() ?? string.Empty}";

            if (match.MatchResult != null)
            {
                var homeScore = BuildScore(match.MatchResult.EventStatus, match.MatchResult.HomeScore);
                var awayScore = BuildScore(match.MatchResult.EventStatus, match.MatchResult.AwayScore);
                DisplayScore = $"{homeScore} - {awayScore}";
            }
        }

        private static string BuildScore(MatchStatus matchStatus, int score)
        {
            if (matchStatus == null)
            {
                return string.Empty;
            }

            return matchStatus.IsPreMatch ? string.Empty : score.ToString();
        }

        private void BuildViewModel(IMatch match)
        {
            MatchViewModel = new MatchViewModel(match, NavigationService, DependencyResolver, EventAggregator, matchHubConnection, true);
            MatchViewModel.BuildMatchStatus();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Not use dispose method because of keeping long using object, handling object is implemented in Clean()
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}