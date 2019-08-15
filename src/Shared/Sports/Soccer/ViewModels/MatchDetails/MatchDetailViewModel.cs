using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using Core.ViewModels;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
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

        private static readonly IList<MatchFunction> TabFunctions = new List<MatchFunction>
                {
                    new MatchFunction { Abbreviation = "Odds", Name = "Odds" },
                    new MatchFunction { Abbreviation = "Info", Name = "Match Info" },
                    new MatchFunction { Abbreviation = "Tracker", Name = "Tracker" },
                    new MatchFunction { Abbreviation = "Stats", Name = "Statistics" },
                    new MatchFunction { Abbreviation = "Line-ups", Name = "Line-ups" },
                    new MatchFunction { Abbreviation = "H2H", Name = "Head to Head" },
                    new MatchFunction { Abbreviation = "Table", Name = "Table" },
                    new MatchFunction { Abbreviation = "Social", Name = "Social" },
                    new MatchFunction { Abbreviation = "TV", Name = "TV Schedule" }
                };

        private readonly HubConnection matchHubConnection;
        private readonly HubConnection teamHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;
        private bool disposedValue;
        private Dictionary<string, TabItemViewModelBase> tabItemViewModels;
        private readonly IMatchStatusConverter matchStatusConverter;
        private string CurrentTabView;

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

            matchStatusConverter = DependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public ObservableCollection<TabItemViewModelBase> TabViews { get; private set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                tabItemViewModels = new Dictionary<string, TabItemViewModelBase>
                {
                    {nameof(Core.Enumerations.MatchFunction.Odds), new DetailOddsViewModel(match.Id, NavigationService, DependencyResolver, new OddsTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.Info), new DetailInfoViewModel(match.Id, NavigationService, DependencyResolver, matchHubConnection, new InfoTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.H2H), new DetailH2HViewModel(NavigationService, DependencyResolver, new H2HTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.Lineups),  new DetailLineupsViewModel(NavigationService, DependencyResolver, new LinesUpTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.Social), new DetailSocialViewModel(NavigationService, DependencyResolver, new SocialTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.Stats), new DetailStatsViewModel(NavigationService, DependencyResolver, new StatisticsTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.Table), new DetailTableViewModel(NavigationService, DependencyResolver, new TableTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.TV), new DetailTVViewModel(NavigationService, DependencyResolver, new TVTemplate()) },
                    {nameof(Core.Enumerations.MatchFunction.Tracker), new DetailTrackerViewModel(NavigationService, DependencyResolver, new TrackerTemplate()) }
                };

                Title = tabItemViewModels.First().Key;
                CurrentTabView = tabItemViewModels.First().Key;

                BuildGeneralInfo(match);
            }
        }

        protected override void Clean()
        {
            base.Clean();

            cancellationTokenSource?.Dispose();

            MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), "TabChange");
        }

        protected override void Initialize()
        {
            BuildTabFunctions();

            cancellationTokenSource = new CancellationTokenSource();

            matchService.SubscribeMatchEvent(matchHubConnection, OnReceivedMatchEvent);
            matchService.SubscribeTeamStatistic(teamHubConnection, OnReceivedTeamStatistic);

            Device.BeginInvokeOnMainThread(async () =>
                await teamHubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token));

            Device.BeginInvokeOnMainThread(async () =>
                await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token));
        }

        public override void OnResume()
        {
            tabItemViewModels[CurrentTabView].OnResume();

            base.OnResume();
        }

        private void BuildTabFunctions()
        {
            TabViews = new ObservableCollection<TabItemViewModelBase>();

            foreach (var tab in TabFunctions)
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
                CurrentTabView = TabViews[index].TabHeaderTitle;
            });
        }

        protected internal void OnReceivedMatchEvent(byte sportId, IMatchEvent matchEvent)
        {
            var match = MatchViewModel.Match;

            if (sportId != CurrentSportId || match?.Id == null || matchEvent.MatchId != match.Id)
            {
                return;
            }

            match.MatchResult = matchEvent.MatchResult;
            match.LatestTimeline = matchEvent.Timeline;

            BuildGeneralInfo(match);
        }

        protected internal void OnReceivedTeamStatistic(byte sportId, string matchId, bool isHome, ITeamStatistic teamStats)
        {
            if (sportId != CurrentSportId || MatchViewModel.Match?.Id == null || MatchViewModel.Match.Id != matchId)
            {
                return;
            }

            MatchViewModel.OnReceivedTeamStatistic(isHome, teamStats);
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

            if (!string.IsNullOrEmpty(winnerId) && match.MatchResult.EventStatus.IsClosed)
            {
                DisplaySecondLeg = $"{AppResources.SecondLeg} {match.MatchResult.AggregateHomeScore} - {match.MatchResult.AggregateAwayScore}";
            }
        }

        private void BuildScoreAndEventDate(IMatch match)
        {
            DisplayEventDate = match.EventDate.ToLocalShortDayMonth();
        }

        private void BuildViewModel(IMatch match)
            => MatchViewModel = new MatchViewModel(match, matchHubConnection, matchStatusConverter, CurrentSportId);

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