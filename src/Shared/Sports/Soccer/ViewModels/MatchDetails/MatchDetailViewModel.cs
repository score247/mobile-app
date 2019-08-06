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
    using LiveScore.Soccer.ViewModels.DetailH2H;
    using LiveScore.Soccer.ViewModels.DetailLineups;
    using LiveScore.Soccer.ViewModels.DetailOdds;
    using LiveScore.Soccer.ViewModels.DetailSocial;
    using LiveScore.Soccer.ViewModels.DetailStats;
    using LiveScore.Soccer.ViewModels.DetailTable;
    using LiveScore.Soccer.ViewModels.DetailTracker;
    using LiveScore.Soccer.ViewModels.DetailTV;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using LiveScore.Soccer.Views.Templates.DetailH2H;
    using LiveScore.Soccer.Views.Templates.DetailInfo;
    using LiveScore.Soccer.Views.Templates.DetailLinesUp;
    using LiveScore.Soccer.Views.Templates.DetailOdds;
    using LiveScore.Soccer.Views.Templates.DetailSocial;
    using LiveScore.Soccer.Views.Templates.DetailStatistics;
    using LiveScore.Soccer.Views.Templates.DetailTable;
    using LiveScore.Soccer.Views.Templates.DetailTracker;
    using LiveScore.Soccer.Views.Templates.DetailTV;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MatchDetailViewModel : ViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);
        private readonly HubConnection matchHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;
        private bool disposedValue;
        private Dictionary<string, TabItemViewModelBase> tabItemViewModels;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            IHubService hubService)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            matchHubConnection = hubService.BuildMatchHubConnection();
            matchService = DependencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.DisplayName);
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDate { get; set; }

        public string DisplaySecondLeg { get; private set; }

        public ObservableCollection<TabItemViewModelBase> TabViews { get; private set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                tabItemViewModels = new Dictionary<string, TabItemViewModelBase>
                {
                    {nameof(MatchFunctions.Odds), new DetailOddsViewModel(match.Id, NavigationService, DependencyResolver, new OddsTemplate()) },
                    {nameof(MatchFunctions.Info), new DetailInfoViewModel(match.Id, NavigationService, DependencyResolver, matchHubConnection, new InfoTemplate()) },
                    {nameof(MatchFunctions.H2H), new DetailH2HViewModel(NavigationService, DependencyResolver, new H2HTemplate()) },
                    {nameof(MatchFunctions.Lineups),  new DetailLineupsViewModel(NavigationService, DependencyResolver, new LinesUpTemplate()) },
                    {nameof(MatchFunctions.Social), new DetailSocialViewModel(NavigationService, DependencyResolver, new SocialTemplate()) },
                    {nameof(MatchFunctions.Stats), new DetailStatsViewModel(NavigationService, DependencyResolver, new StatisticsTemplate()) },
                    {nameof(MatchFunctions.Table), new DetailTableViewModel(NavigationService, DependencyResolver, new TableTemplate()) },
                    {nameof(MatchFunctions.TV), new DetailTVViewModel(NavigationService, DependencyResolver, new TVTemplate()) },
                    {nameof(MatchFunctions.Tracker), new DetailTrackerViewModel(NavigationService, DependencyResolver, new TrackerTemplate()) }
                };

                Title = tabItemViewModels.First().Key;

                BuildGeneralInfo(match);
            }
        }

        protected override void Clean()
        {
            base.Clean();

            cancellationTokenSource?.Dispose();

            MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), "TabChange");
        }

        protected override async void Initialize()
        {
            try
            {
                await LoadData(() => LoadMatchDetail(MatchViewModel.Match.Id));

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
            TabViews = new ObservableCollection<TabItemViewModelBase>();

            if (match.Functions != null)
            {
                TabViews = new ObservableCollection<TabItemViewModelBase>();

                foreach (var tab in match.Functions)
                {
                    var tabName = tab.Abbreviation.Replace("-", string.Empty);

                    if (tabItemViewModels.ContainsKey(tabName))
                    {
                        var tabModel = tabItemViewModels[tabName];
                        tabModel.Title = tab.Name;
                        tabModel.TabHeaderTitle = tab.Abbreviation;

                        TabViews.Add(tabModel);
                    }
                }

                MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", (_, index) =>
                {
                    Title = TabViews[index].Title;
                });
            }
        }

        private async Task StartListeningMatchHubEvent()
        {
            matchHubConnection.On<byte, Dictionary<string, MatchPushEvent>>("PushMatchEvent", OnReceivingMatchEvent);

            await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, cancellationTokenSource.Token);
        }

        protected internal void OnReceivingMatchEvent(byte sportId, Dictionary<string, MatchPushEvent> payload)
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
        }

        private void BuildSecondLeg(IMatch match)
        {
            var winnerId = match.MatchResult?.AggregateWinnerId;

            if (!string.IsNullOrEmpty(winnerId) && match.MatchResult.EventStatus.IsClosed())
            {
                DisplaySecondLeg = $"{AppResources.SecondLeg} {match.MatchResult.AggregateHomeScore} - {match.MatchResult.AggregateAwayScore}";
            }
        }

        private void BuildScoreAndEventDate(IMatch match)
        {
            DisplayEventDate = match.EventDate.ToShortDayMonth();
        }

        private void BuildViewModel(IMatch match)
        {
            MatchViewModel = new MatchViewModel(match, NavigationService, DependencyResolver, EventAggregator, matchHubConnection);
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