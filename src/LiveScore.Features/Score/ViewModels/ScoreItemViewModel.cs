﻿[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Features.Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Helpers;
    using Core;
    using Core.Converters;
    using Core.PubSubEvents.Matches;
    using Core.PubSubEvents.Teams;
    using Core.Services;
    using LiveScore.Core.ViewModels;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class ScoreItemViewModel : ViewModelBase
    {
        private static readonly ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> EmptyMatchDataSource =
            new ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(Enumerable.Empty<IGrouping<GroupMatchViewModel, MatchViewModel>>().ToList());

        private readonly IMatchService matchService;
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly IMatchMinuteConverter matchMinuteConverter;
        private bool firstLoad = true;

        [Time]
        public ScoreItemViewModel(
            DateTime selectedDate,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = selectedDate;

            matchStatusConverter = dependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
            matchMinuteConverter = dependencyResolver.Resolve<IMatchMinuteConverter>(CurrentSportId.ToString());
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());

            InitializeCommand();
            SubscribeEvents();
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; private set; }

        public DelegateAsyncCommand ClickSearchCommand { get; private set; }

        [Time]
        public override async void OnResume()
        {
            Profiler.Start("ScoreItemViewModel.OnResume");

            SubscribeEvents();
            await InitializeData();
        }

        public override void OnSleep()
        {
            UnsubscribeAllEvents();
        }

        public override async void OnAppearing()
        {
            if (firstLoad)
            {
                await InitializeData();
                firstLoad = false;
            }
        }

        private void InitializeCommand()
        {
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatch);
            ClickSearchCommand = new DelegateAsyncCommand(OnClickSearch);
        }

        [Time]
        private async Task InitializeData()
        {
            await LoadData(() => LoadMatches(SelectedDate, true)).ConfigureAwait(false);
        }

        private void SubscribeEvents()
        {
            EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Subscribe(OnReceivedMatchEvent, true);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Subscribe(OnReceivedTeamStatistic, true);
        }

        private void UnsubscribeAllEvents()
        {
            EventAggregator
               .GetEvent<MatchEventPubSubEvent>()
               .Unsubscribe(OnReceivedMatchEvent);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Unsubscribe(OnReceivedTeamStatistic);
        }

        private async Task OnRefresh()
        {
            Profiler.Start("ScoreItemViewModel.LoadMatches.PullDownToRefresh");

            await LoadData(() => LoadMatches(SelectedDate, true), false)
                .ConfigureAwait(false);
        }

        private async Task OnTapMatch(MatchViewModel matchItem)
        {
            // TODO: Change to use IAutoInitialize for parameters followed by new release of prism
            // https://github.com/PrismLibrary/Prism/releases
            var parameters = new NavigationParameters
            {
                { "Match", matchItem.Match }
            };

            var navigated = await NavigationService
                .NavigateAsync("MatchDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        private async Task OnClickSearch()
            => await NavigationService
                .NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true)
                .ConfigureAwait(false);

        [Time]
        private async Task LoadMatches(DateTime date, bool forceFetchNewData = false)
        {
            await Task.Run(() => UnsubscribeLiveMatchTimeChangeEvent(date)).ConfigureAwait(false);

            if (IsLoading)
            {
                MatchItemsSource = EmptyMatchDataSource;
            }

            await GetMatches(date, forceFetchNewData);

            Profiler.Stop("ScoreItemViewModel.LoadMatches.PullDownToRefresh");
        }

        private async Task GetMatches(DateTime date, bool forceFetchNewData)
        {
            var matches = await matchService.GetMatchesByDate(
                    date,
                    CurrentLanguage,
                    forceFetchNewData).ConfigureAwait(false);

            var matchItemViewModels = matches.Select(
                match => new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator));

            MatchItemsSource = new ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match)).ToList());

            IsRefreshing = false;
            IsLoading = false;

            Profiler.Start("ScoresView.Render");
            Debug.WriteLine($"Number of matches: {matches.Count()}");
        }

        private void UnsubscribeLiveMatchTimeChangeEvent(DateTime date)
        {
            // TODO: Enhance later - Call Dispose() for closing Subscriber Match Time Event
            if ((date == DateTime.Today || date == DateTimeExtension.Yesterday().Date)
                    && MatchItemsSource?.Count > 0)
            {
                var liveMatchViewModels = MatchItemsSource
                        .SelectMany(group => group)
                        .Where(matchItem => matchItem.Match.EventStatus?.IsLive == true);

                // experiment parallel
                Parallel.ForEach(liveMatchViewModels, (viewModel, _)
                    => viewModel.UnsubscribeMatchTimeChangeEvent());
            }
        }

        internal void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            if (payload?.SportId != CurrentSportId)
            {
                return;
            }

            var matchItem = MatchItemsSource
                .SelectMany(group => group)
                .FirstOrDefault(m => m.Match.Id == payload.MatchEvent.MatchId);

            if (matchItem?.Match != null)
            {
                matchItem.OnReceivedMatchEvent(payload.MatchEvent);
            }
        }

        internal void OnReceivedTeamStatistic(ITeamStatisticsMessage payload)
        {
            if (payload.SportId != CurrentSportId)
            {
                return;
            }

            var matchItem = MatchItemsSource
                .SelectMany(group => group)
                .FirstOrDefault(m => m.Match.Id == payload.MatchId);

            matchItem?.OnReceivedTeamStatistic(payload.IsHome, payload.TeamStatistic);
        }
    }
}