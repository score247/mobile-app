﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Controls.TabStrip.EventArgs;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.Information;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.Statistics;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.TrackerCommentary;
using LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Table;
using LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead;
using LiveScore.Soccer.Views.Templates.MatchDetails.Information;
using LiveScore.Soccer.Views.Templates.MatchDetails.LineUps;
using LiveScore.Soccer.Views.Templates.MatchDetails.Statistics;
using LiveScore.Soccer.Views.Templates.MatchDetails.TrackerCommentary;
using MethodTimer;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.Matches
{
    public class MatchDetailViewModel : ViewModelBase
    {
        private static readonly DataTemplate infoTemplate = new InformationTemplate();
        private static readonly DataTemplate h2hTemplate = new H2HTemplate();
        private static readonly DataTemplate lineupsTemplate = new LineUpsTemplate();
        private static readonly DataTemplate statisticsTemplate = new StatisticsTemplate();
        private static readonly DataTemplate trackerTemplate = new TrackerCommentaryTemplate();
        private static readonly DataTemplate tableTemplate = new TableTemplate();

        private readonly IMatchDisplayStatusBuilder matchStatusConverter;
        private readonly IMatchMinuteBuilder matchMinuteConverter;
        private readonly Func<string, string> buildFlagUrlFunc;
        private readonly IFavoriteService favoriteService;
        private MatchDetailFunction selectedTabItem;
        private readonly ISoccerMatchService soccerMatchService;
        private IDictionary<MatchDetailFunction, TabItemViewModel> tabItemViewModels;
        private string currentMatchId;
        private DateTimeOffset currentMatchEventDate;
        private MatchStatus currentMatchStatus;
        private bool firstLoad = true;

        public MatchDetailViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            IFavoriteService favoriteService)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            matchStatusConverter = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            matchMinuteConverter = DependencyResolver.Resolve<IMatchMinuteBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            this.favoriteService = favoriteService;

            FunctionTabTappedCommand = new DelegateCommand<TabStripItemTappedEventArgs>(OnFunctionTabTapped);
        }

        public MatchViewModel MatchViewModel { get; private set; }

        public string CountryFlag { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplaySecondLeg { get; private set; }

        public ObservableCollection<TabItemViewModel> TabItems { get; private set; }

        public DelegateCommand<TabStripItemTappedEventArgs> FunctionTabTappedCommand { get; }

        public override async void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Match"] is IMatch match)
            {
                currentMatchId = match.Id;
                currentMatchEventDate = match.EventDate;
                currentMatchStatus = match.EventStatus;

                BuildGeneralInfo(match);
                TabItems = new ObservableCollection<TabItemViewModel>(
                    await GenerateTabItemViewModels(MatchViewModel.Match)
                    .ConfigureAwait(false));
                CountryFlag = buildFlagUrlFunc(MatchViewModel.Match.CountryCode);
            }
        }

        [Time]
        public override async void OnAppearing()
        {
            if (selectedTabItem != null)
            {
                tabItemViewModels[selectedTabItem]?.OnAppearing();
            }

            if (!firstLoad && (currentMatchStatus?.IsLive == true))
            {
                var matchInfo = await GetMatch(currentMatchId);

                if (matchInfo?.Match != null)
                {
                    BuildViewModel(matchInfo.Match);
                }
            }

            SubscribeEvents();
            firstLoad = false;
        }

        public override async void OnResumeWhenNetworkOK()
        {
            if (selectedTabItem != null)
            {
                tabItemViewModels[selectedTabItem]?.OnResumeWhenNetworkOK();
            }

            var matchInfo = await GetMatch(currentMatchId);

            if (matchInfo?.Match != null)
            {
                BuildViewModel(matchInfo.Match);
            }

            SubscribeEvents();
        }

        public override void OnSleep()
        {
            if (selectedTabItem != null)
            {
                tabItemViewModels[selectedTabItem]?.OnSleep();
            }

            UnSubscribeEvents();
        }

        public override void OnDisappearing()
        {
            if (selectedTabItem != null)
            {
                tabItemViewModels[selectedTabItem]?.OnDisappearing();
            }

            UnSubscribeEvents();
        }

        public override async Task OnNetworkReconnectedAsync()
        {
            if (selectedTabItem != null)
            {
                await tabItemViewModels[selectedTabItem]?.OnNetworkReconnectedAsync();
            }

            var matchInfo = await GetMatch(currentMatchId);

            if (matchInfo?.Match != null)
            {
                BuildViewModel(matchInfo.Match);
            }
        }

        private void SubscribeEvents()
        {
            if (EventAggregator == null || MatchViewModel?.Match == null)
            {
                return;
            }

            if (MatchViewModel.Match.EventStatus.IsLive || MatchViewModel.Match.EventStatus.IsNotStarted)
            {
                EventAggregator
                    .GetEvent<MatchEventPubSubEvent>()
                    .Subscribe(OnReceivedMatchEvent);

                EventAggregator
                    .GetEvent<MatchEventRemovedPubSubEvent>()
                    .Subscribe(OnReceivedMatchEventRemoved);

                EventAggregator
                    .GetEvent<TeamStatisticPubSubEvent>()
                    .Subscribe(OnReceivedTeamStatistic);
            }
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
                .GetEvent<MatchEventRemovedPubSubEvent>()
                .Unsubscribe(OnReceivedMatchEventRemoved);

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

#pragma warning disable S3168 // "async" methods should not return "void"

        protected internal async void OnReceivedMatchEventRemoved(IMatchEventRemovedMessage payload)
        {
            if (MatchViewModel.Match.Id != payload.MatchId)
            {
                return;
            }

            Debug.WriteLine($"OnReceivedMatchEventRemoved {payload.MatchId} - need to reload");

            var matchInfo = await GetMatch(currentMatchId);

            if (matchInfo?.Match != null)
            {
                BuildViewModel(matchInfo.Match);
            }
        }

#pragma warning restore S3168 // "async" methods should not return "void"

        private void BuildGeneralInfo(IMatch match)
        {
            BuildViewModel(match);
            BuildSecondLeg(match);

            DisplayEventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
        }

        private void BuildSecondLeg(IMatch match)
        {
            if (match is SoccerMatch soccerMatch)
            {
                var winnerId = soccerMatch.AggregateWinnerId;

                if (!string.IsNullOrWhiteSpace(winnerId) && soccerMatch.EventStatus.IsClosed)
                {
                    DisplaySecondLeg = $"{AppResources.SecondLeg} {soccerMatch.AggregateHomeScore} - {soccerMatch.AggregateAwayScore}";
                }
            }
        }

        private void BuildViewModel(IMatch match)
            => MatchViewModel = new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator, favoriteService);

        [Time]
        private async Task<List<TabItemViewModel>> GenerateTabItemViewModels(IMatch match)
        {
            var coverage = await soccerMatchService.GetMatchCoverageAsync(
                    MatchViewModel.Match.Id,
                    CurrentLanguage,
                    currentMatchEventDate,
                    forceFetchLatestData: true).ConfigureAwait(false);

            var viewModels = new List<TabItemViewModel>();

            tabItemViewModels = new Dictionary<MatchDetailFunction, TabItemViewModel>
            {
                [MatchDetailFunction.Info] = new InformationViewModel(match, NavigationService, DependencyResolver, EventAggregator, infoTemplate),
                [MatchDetailFunction.H2H] = new H2HViewModel(match, NavigationService, DependencyResolver, EventAggregator, h2hTemplate),
                [MatchDetailFunction.Lineups] = new LineupsViewModel(match.Id, match.EventDate, NavigationService, DependencyResolver, EventAggregator, lineupsTemplate),
                [MatchDetailFunction.Stats] = new StatisticsViewModel(match.Id, match.EventDate, NavigationService, DependencyResolver, EventAggregator, statisticsTemplate),
                [MatchDetailFunction.Table] = new TableViewModel(
                    match.LeagueId,
                    match.LeagueSeasonId,
                    match.LeagueRoundGroup,
                    NavigationService,
                    DependencyResolver,
                    tableTemplate,
                    homeTeamId: match.HomeTeamId,
                    awayTeamId: match.AwayTeamId,
                    highlightTeamName: true),
                [MatchDetailFunction.Tracker] = new TrackerCommentaryViewModel(coverage, match.EventDate, NavigationService, DependencyResolver, EventAggregator, trackerTemplate)
            };

            Title = tabItemViewModels.First().Key.DisplayName;
            selectedTabItem = tabItemViewModels.First().Key;

            // Temporary show all functions
            foreach (var function in Enumeration.GetAll<MatchDetailFunction>())
            {
                if (tabItemViewModels.ContainsKey(function))
                {
                    var tabModel = tabItemViewModels[function];

                    viewModels.Add(tabModel);
                }
            }

            return viewModels;
        }

        private void OnFunctionTabTapped(TabStripItemTappedEventArgs args)
        {
            if (args != null)
            {
                selectedTabItem = Enumeration.FromValue<MatchDetailFunction>(args.Index);
            }
        }

        private Task<MatchInfo> GetMatch(string id)
            => soccerMatchService.GetMatchAsync(id, CurrentLanguage, currentMatchEventDate);
    }
}