using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Core.ViewModels;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
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
    using MethodTimer;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MatchDetailViewModel : ViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private static readonly IList<MatchFunction> TabFunctions = new List<MatchFunction>
        {
            new MatchFunction("Odds", "Odds"),
            new MatchFunction("Info", "Match Info"),
            new MatchFunction("Tracker", "Tracker"),
            new MatchFunction("Stats", "Statistics"),
            new MatchFunction("Line-ups", "Line-ups"),
            new MatchFunction("H2H", "Head to Head"),
            new MatchFunction("Table", "Table"),
            new MatchFunction("Social", "Social"),
            new MatchFunction("TV", "TV Schedule")
        };

        private readonly HubConnection matchHubConnection;
        private readonly HubConnection teamHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;
        private bool disposedValue;
        private Dictionary<TabFunction, TabItemViewModelBase> tabItemViewModels;
        private TabFunction CurrentTabView;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            var hubService = DependencyResolver.Resolve<IHubService>(CurrentSportId.ToString());
            matchHubConnection = hubService.BuildMatchEventHubConnection();
            teamHubConnection = hubService.BuildTeamStatisticHubConnection();
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public ObservableCollection<TabItemViewModelBase> TabViews { get; private set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                tabItemViewModels = new Dictionary<TabFunction, TabItemViewModelBase>
                {
                    {TabFunction.Odds, new DetailOddsViewModel(match.Id, match.MatchResult.EventStatus, NavigationService, DependencyResolver, new OddsTemplate()) },
                    {TabFunction.Info, new DetailInfoViewModel(match.Id, NavigationService, DependencyResolver, matchHubConnection, new InfoTemplate()) },
                    {TabFunction.H2H, new DetailH2HViewModel(NavigationService, DependencyResolver, new H2HTemplate()) },
                    {TabFunction.Lineups,  new DetailLineupsViewModel(NavigationService, DependencyResolver, new LinesUpTemplate()) },
                    {TabFunction.Social, new DetailSocialViewModel(NavigationService, DependencyResolver, new SocialTemplate()) },
                    {TabFunction.Stats, new DetailStatsViewModel(NavigationService, DependencyResolver, new StatisticsTemplate()) },
                    {TabFunction.Table, new DetailTableViewModel(NavigationService, DependencyResolver, new TableTemplate()) },
                    {TabFunction.TV, new DetailTVViewModel(NavigationService, DependencyResolver, new TVTemplate()) },
                    {TabFunction.Tracker, new DetailTrackerViewModel(NavigationService, DependencyResolver, new TrackerTemplate()) }
                };

                Title = tabItemViewModels.First().Key.DisplayName;
                CurrentTabView = tabItemViewModels.First().Key;

                InitViewModel(match);
                BuildGeneralInfo();
            }
        }

        protected override void Clean()
        {
            base.Clean();

            cancellationTokenSource?.Cancel();

            MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), "TabChange");
        }

        [Time]
        protected override async void Initialize()
        {
            BuildTabFunctions();

            cancellationTokenSource = new CancellationTokenSource();

            matchService.SubscribeMatchEvent(matchHubConnection, OnReceivedMatchEvent);
            matchService.SubscribeTeamStatistic(teamHubConnection, OnReceivedTeamStatistic);

            var match = await matchService.GetMatch(MatchViewModel.Match.Id, CurrentLanguage, true);
            MatchViewModel.UpdateMatch(match);

            Device.BeginInvokeOnMainThread(async () =>
                await teamHubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token));

            Device.BeginInvokeOnMainThread(async () =>
                await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token));
        }

        public override void OnResume()
        {
            Debug.WriteLine("MatchDetailViewModel OnResume");

            tabItemViewModels[CurrentTabView].OnResume();

            base.OnResume();
        }

        public override void OnSleep()
        {
            tabItemViewModels[CurrentTabView].OnSleep();

            base.OnSleep();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            foreach (var tab in tabItemViewModels)
            {
                tab.Value.OnDisappearing();
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            foreach (var tab in tabItemViewModels)
            {
                tab.Value.Destroy();
            }
        }

        [Time]
        protected internal void OnReceivedMatchEvent(byte sportId, IMatchEvent matchEvent)
        {
            var match = MatchViewModel.Match;

            if (sportId != CurrentSportId || match?.Id == null || matchEvent.MatchId != match.Id)
            {
                return;
            }

            MatchViewModel.OnReceivedMatchEvent(matchEvent);

            BuildGeneralInfo();
        }

        protected internal void OnReceivedTeamStatistic(byte sportId, string matchId, bool isHome, ITeamStatistic teamStats)
        {
            if (sportId != CurrentSportId || MatchViewModel.Match?.Id == null || MatchViewModel.Match.Id != matchId)
            {
                return;
            }

            MatchViewModel.OnReceivedTeamStatistic(isHome, teamStats);
        }

        private void BuildTabFunctions()
        {
            TabViews = new ObservableCollection<TabItemViewModelBase>();

            foreach (var tab in TabFunctions)
            {
                var tabFunction = Enumeration.FromDisplayName<TabFunction>(tab.Abbreviation);

                if (tabItemViewModels.ContainsKey(tabFunction))
                {
                    var tabModel = tabItemViewModels[tabFunction];
                    tabModel.Title = tab.Name;
                    tabModel.TabHeaderTitle = tab.Abbreviation;

                    TabViews.Add(tabModel);
                }
            }

            MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", (_, index) =>
            {
                Title = TabViews[index].Title;
                CurrentTabView = Enumeration.FromDisplayName<TabFunction>(TabViews[index].TabHeaderTitle);
            });
        }


        private void BuildGeneralInfo()
        {
            BuildScoreAndEventDate();

            BuildSecondLeg();
        }

        private void BuildSecondLeg()
        {
            var match = MatchViewModel.Match;
            var winnerId = match.MatchResult?.AggregateWinnerId;

            if (!string.IsNullOrEmpty(winnerId) && match.MatchResult.EventStatus.IsClosed)
            {
                DisplaySecondLeg = $"{AppResources.SecondLeg} {match.MatchResult.AggregateHomeScore} - {match.MatchResult.AggregateAwayScore}";
            }
        }

        private void BuildScoreAndEventDate()
        {
            DisplayEventDate = MatchViewModel.Match.EventDate.ToLocalShortDayMonth();
        }

        private void InitViewModel(IMatch match)
            => MatchViewModel = new MatchViewModel(match, DependencyResolver, CurrentSportId);

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