using System.Collections.Generic;
using System.Collections.ObjectModel;
    using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.PubSubEvents.Matches;
    using LiveScore.Soccer.Models.Matches;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Core.ViewModels;
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

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels
{
    public class MatchDetailViewModel : ViewModelBase
    {
        private MatchDetailFunction selectedTabItem;
        private IDictionary<MatchDetailFunction, TabItemViewModel> tabItemViewModels;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public IList<TabItemViewModel> TabItems { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                BuildTabItems(match);
                    {TabFunction.Odds, new DetailOddsViewModel(match.Id, match.MatchResult.EventStatus, NavigationService, DependencyResolver, new OddsTemplate()) },
                InitViewModel(match);
                BuildGeneralInfo();
            }
        }

            cancellationTokenSource?.Cancel();
        protected override async void Initialize()
            var match = await matchService.GetMatch(MatchViewModel.Match.Id, CurrentLanguage, true);
            MatchViewModel.BuildMatch(match);

        public override void OnResume()
        {
            Debug.WriteLine("MatchDetailViewModel OnResume");


            base.OnResume();
        }

        public override void OnSleep()
        {
            tabItemViewModels[selectedTabItem].OnSleep();

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

        protected override void OnDisposed()
        {
            base.OnDisposed();

            EventAggregator
              .GetEvent<MatchEventPubSubEvent>()
              .Unsubscribe(OnReceivedMatchEvent);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Unsubscribe(OnReceivedTeamStatistic);

            MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), "TabChange");
        }

        protected internal void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            var match = MatchViewModel.Match;

            if (payload.SportId != CurrentSportId || payload.MatchEvent.MatchId != match.Id)
            {
                return;
            }

            MatchViewModel.OnReceivedMatchEvent(matchEvent);

            BuildGeneralInfo();
        }

        protected internal void OnReceivedTeamStatistic(ITeamStatisticsMessage payload)
        {
            if (payload.SportId != CurrentSportId || MatchViewModel.Match.Id != payload.MatchId)
            {
                return;
            }

            MatchViewModel.OnReceivedTeamStatistic(payload.IsHome, payload.TeamStatistic);
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

                if (!string.IsNullOrWhiteSpace(winnerId) && soccerMatch.EventStatus.IsClosed)
                {
                    DisplaySecondLeg = $"{AppResources.SecondLeg} {soccerMatch.AggregateHomeScore} - {soccerMatch.AggregateAwayScore}";
                }
            }
        }

        private void BuildScoreAndEventDate()

        private void BuildTabItems(IMatch match)
        {
            DisplayEventDate = MatchViewModel.Match.EventDate.ToLocalShortDayMonth();
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

        private void InitViewModel(IMatch match)
            => MatchViewModel = new MatchViewModel(match, DependencyResolver, CurrentSportId);
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