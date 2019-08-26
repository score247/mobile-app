using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.ViewModels;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
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
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MatchDetailViewModel : ViewModelBase
    {
        private readonly IMatchService matchService;
        private MatchDetailFunction selectedTabItem;
        private IDictionary<MatchDetailFunction, TabItemViewModel> tabItemViewModels;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public IList<TabItemViewModel> TabItems { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                var soccerMatch = match as Match;

                BuildTabItems(soccerMatch);
                BuildGeneralInfo(soccerMatch);
            }
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), "TabChange");
        }

        protected override void OnInitialized()
        {
            TabItems = new ObservableCollection<TabItemViewModel>(TabItems);

            // TODO: Add subscribe signalR here

            MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", (_, index) =>
            {
                Title = TabItems[index].Title;
                selectedTabItem = TextEnumeration.FromValue<MatchDetailFunction>(TabItems[index].TabHeaderTitle);
            });
        }

        public override void OnResume()
        {
            tabItemViewModels[selectedTabItem].OnResume();

            base.OnResume();
        }

        public override void OnSleep()
        {
            tabItemViewModels[selectedTabItem].OnSleep();

            base.OnSleep();
        }

        protected internal void OnReceivedMatchEvent(byte sportId, IMatchEvent matchEvent)
        {
            var match = MatchViewModel.Match as Match;

            if (sportId != CurrentSportId || match?.Id == null || matchEvent.MatchId != match.Id)
            {
                return;
            }

            match.UpdateResult(matchEvent.MatchResult);
            match.UpdateLastTimeline(matchEvent.Timeline);

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

        private void BuildGeneralInfo(Match match)
        {
            BuildViewModel(match);
            BuildSecondLeg(match);

            DisplayEventDate = match.EventDate.ToLocalShortDayMonth();
        }

        private void BuildSecondLeg(Match match)
        {
            var winnerId = match.AggregateWinnerId;

            if (!string.IsNullOrEmpty(winnerId) && match.EventStatus.IsClosed)
            {
                DisplaySecondLeg = $"{AppResources.SecondLeg} {match.AggregateHomeScore} - {match.AggregateAwayScore}";
            }
        }

        private void BuildViewModel(Match match) => MatchViewModel = new MatchViewModel(match, DependencyResolver, CurrentSportId);

        private void BuildTabItems(IMatch match)
        {
            tabItemViewModels = new Dictionary<MatchDetailFunction, TabItemViewModel>
            {
                {MatchDetailFunction.Odds, new DetailOddsViewModel(match.Id, NavigationService, DependencyResolver, EventAggregator, new OddsTemplate()) },
                {MatchDetailFunction.Info, new DetailInfoViewModel(match.Id, NavigationService, DependencyResolver, EventAggregator, new InfoTemplate()) },
                {MatchDetailFunction.H2H, new DetailH2HViewModel(NavigationService, DependencyResolver, new H2HTemplate()) },
                {MatchDetailFunction.Lineups,  new DetailLineupsViewModel(NavigationService, DependencyResolver, new LinesUpTemplate()) },
                {MatchDetailFunction.Social, new DetailSocialViewModel(NavigationService, DependencyResolver, new SocialTemplate()) },
                {MatchDetailFunction.Stats, new DetailStatsViewModel(NavigationService, DependencyResolver, new StatisticsTemplate()) },
                {MatchDetailFunction.Table, new DetailTableViewModel(NavigationService, DependencyResolver, new TableTemplate()) },
                {MatchDetailFunction.TV, new DetailTVViewModel(NavigationService, DependencyResolver, new TVTemplate()) },
                {MatchDetailFunction.Tracker, new DetailTrackerViewModel(NavigationService, DependencyResolver, new TrackerTemplate()) }
            };

            Title = tabItemViewModels.First().Key.DisplayName;
            selectedTabItem = tabItemViewModels.First().Key;
            TabItems = new List<TabItemViewModel>();

            // Temporary show all functions
            foreach (var function in TextEnumeration.GetAll<MatchDetailFunction>())
            {
                if (tabItemViewModels.ContainsKey(function))
                {
                    var tabModel = tabItemViewModels[function];
                    tabModel.Title = function.DisplayName;
                    tabModel.TabHeaderTitle = function.Value;

                    TabItems.Add(tabModel);
                }
            }
        }
    }
}