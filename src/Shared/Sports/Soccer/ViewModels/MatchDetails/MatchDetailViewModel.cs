using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Controls.TabStrip.EventArgs;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.DetailH2H;
using LiveScore.Soccer.ViewModels.DetailLineups;
using LiveScore.Soccer.ViewModels.DetailSocial;
using LiveScore.Soccer.ViewModels.DetailStats;
using LiveScore.Soccer.ViewModels.DetailTable;
using LiveScore.Soccer.ViewModels.DetailTV;
using LiveScore.Soccer.ViewModels.MatchDetailInfo;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailTracker;
using LiveScore.Soccer.Views.Templates.DetailH2H;
using LiveScore.Soccer.Views.Templates.DetailInfo;
using LiveScore.Soccer.Views.Templates.DetailLinesUp;
using LiveScore.Soccer.Views.Templates.DetailSocial;
using LiveScore.Soccer.Views.Templates.DetailStatistics;
using LiveScore.Soccer.Views.Templates.DetailTable;
using LiveScore.Soccer.Views.Templates.DetailTracker;
using LiveScore.Soccer.Views.Templates.DetailTV;
using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds;
using MethodTimer;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails
{
    public class MatchDetailViewModel : ViewModelBase
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly IMatchMinuteConverter matchMinuteConverter;
        private readonly Func<string, string> buildFlagUrlFunc;
        private MatchDetailFunction selectedTabItem;
        private IDictionary<MatchDetailFunction, TabItemViewModel> tabItemViewModels;

        private readonly ISoccerMatchService matchInfoService;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            matchInfoService = DependencyResolver.Resolve<ISoccerMatchService>();

            matchStatusConverter = dependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
            matchMinuteConverter = dependencyResolver.Resolve<IMatchMinuteConverter>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            FunctionTabTappedCommand = new DelegateCommand<TabStripItemTappedEventArgs>(OnFunctionTabTapped);
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string CountryFlag { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public ObservableCollection<TabItemViewModel> TabItems { get; private set; }

        public DelegateCommand<TabStripItemTappedEventArgs> FunctionTabTappedCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                BuildGeneralInfo(match);
                TabItems = new ObservableCollection<TabItemViewModel>(await GenerateTabItemViewModels(MatchViewModel.Match));
                CountryFlag = buildFlagUrlFunc(MatchViewModel.Match.CountryCode);
            }
        }

        public override void OnResumeWhenNetworkOK()
        {
            tabItemViewModels[selectedTabItem].OnResumeWhenNetworkOK();

            SubscribeEvents();
        }

        public override void OnSleep()
        {
            tabItemViewModels[selectedTabItem].OnSleep();

            UnSubscribeEvents();
        }

        public override void OnDisappearing()
        {
            tabItemViewModels[selectedTabItem].OnDisappearing();

            UnSubscribeEvents();
        }

        public override void OnAppearing()
        {
            if (selectedTabItem != null)
            {
                tabItemViewModels[selectedTabItem].OnAppearing();
            }

            SubscribeEvents();
        }

        public override Task OnNetworkReconnected()
            => tabItemViewModels[selectedTabItem].OnNetworkReconnected();

        private void SubscribeEvents()
        {
            if (EventAggregator == null)
            {
                return;
            }

            EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Subscribe(OnReceivedMatchEvent, true);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Subscribe(OnReceivedTeamStatistic, true);
        }

        private void UnSubscribeEvents()
        {
            if (EventAggregator == null)
            {
                return;
            }

            EventAggregator
              .GetEvent<MatchEventPubSubEvent>()
              .Unsubscribe(OnReceivedMatchEvent);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Unsubscribe(OnReceivedTeamStatistic);
        }

        protected internal void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            var match = MatchViewModel.Match;

            if (payload.SportId != CurrentSportId || payload.MatchEvent.MatchId != match.Id)
            {
                return;
            }

            MatchViewModel.OnReceivedMatchEvent(payload.MatchEvent);

            BuildGeneralInfo(match);
        }

        protected internal void OnReceivedTeamStatistic(ITeamStatisticsMessage payload)
        {
            if (payload.SportId != CurrentSportId || MatchViewModel.Match.Id != payload.MatchId)
            {
                return;
            }

            MatchViewModel.OnReceivedTeamStatistic(payload.IsHome, payload.TeamStatistic);
        }

        private void BuildGeneralInfo(IMatch match)
        {
            BuildViewModel(match);
            BuildSecondLeg(match);

            DisplayEventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
        }

        private void BuildSecondLeg(IMatch match)
        {
            if (match is Match soccerMatch)
            {
                var winnerId = soccerMatch.AggregateWinnerId;

                if (!string.IsNullOrWhiteSpace(winnerId) && soccerMatch.EventStatus.IsClosed)
                {
                    DisplaySecondLeg = $"{AppResources.SecondLeg} {soccerMatch.AggregateHomeScore} - {soccerMatch.AggregateAwayScore}";
                }
            }
        }

        private void BuildViewModel(IMatch match)
            => MatchViewModel = new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator);

        [Time]
        private async Task<List<TabItemViewModel>> GenerateTabItemViewModels(IMatch match)
        {
            var coverage = await matchInfoService.GetMatchCoverage(MatchViewModel.Match.Id, CurrentLanguage, forceFetchNewData: true);

            var viewModels = new List<TabItemViewModel>();

            tabItemViewModels = new Dictionary<MatchDetailFunction, TabItemViewModel>
            {
                {MatchDetailFunction.Odds, new DetailOddsViewModel(match.Id, match.EventStatus,  NavigationService, DependencyResolver, EventAggregator, new OddsTemplate()) },
                {MatchDetailFunction.Info, new DetailInfoViewModel(match.Id, NavigationService, DependencyResolver, EventAggregator, new InfoTemplate()) },
                {MatchDetailFunction.H2H, new DetailH2HViewModel(NavigationService, DependencyResolver, new H2HTemplate()) },
                {MatchDetailFunction.Lineups,  new DetailLineupsViewModel(NavigationService, DependencyResolver, new LinesUpTemplate()) },
                {MatchDetailFunction.Social, new DetailSocialViewModel(NavigationService, DependencyResolver, new SocialTemplate()) },
                {MatchDetailFunction.Stats, new DetailStatsViewModel(match.Id, NavigationService, DependencyResolver, EventAggregator, new StatisticsTemplate()) },
                {MatchDetailFunction.Table, new DetailTableViewModel(NavigationService, DependencyResolver, new TableTemplate()) },
                {MatchDetailFunction.TV, new DetailTVViewModel(NavigationService, DependencyResolver, new TVTemplate()) },
                {MatchDetailFunction.Tracker, new DetailTrackerViewModel(coverage, NavigationService, DependencyResolver, EventAggregator, new TrackerTemplate()) }
            };

            Title = tabItemViewModels.First().Key.DisplayName;
            selectedTabItem = tabItemViewModels.First().Key;

            // Temporary show all functions
            foreach (var function in TextEnumeration.GetAll<MatchDetailFunction>())
            {
                if (tabItemViewModels.ContainsKey(function))
                {
                    var tabModel = tabItemViewModels[function];
                    tabModel.Title = function.DisplayName;
                    tabModel.TabHeaderTitle = function.Value;

                    viewModels.Add(tabModel);
                }
            }

            return viewModels;
        }

        private void OnFunctionTabTapped(TabStripItemTappedEventArgs args)
        {
            Title = TabItems[args.Index].Title;
            selectedTabItem = TextEnumeration.FromValue<MatchDetailFunction>(TabItems[args.Index].TabHeaderTitle);
        }
    }
}