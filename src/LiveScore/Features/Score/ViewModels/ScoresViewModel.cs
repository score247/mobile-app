[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Features.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Helpers;
    using Core;
    using Core.Controls.DateBar.Events;
    using Core.Converters;
    using Core.PubSubEvents.Matches;
    using Core.PubSubEvents.Teams;
    using Core.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.ViewModels;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class ScoresViewModel : ViewModelBase
    {
        private readonly IMatchService matchService;
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly IMatchMinuteConverter matchMinuteConverter;
        private DateRange selectedDateRange;
        private CancellationTokenSource cancellationTokenSource;

        private static readonly ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> EmptyMatchDataSource =
            new ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(Enumerable.Empty<IGrouping<GroupMatchViewModel, MatchViewModel>>().ToList());

        [Time]
        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = DateTime.Today;

            matchStatusConverter = dependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
            matchMinuteConverter = dependencyResolver.Resolve<IMatchMinuteConverter>(CurrentSportId.ToString());
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());

            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatch);
            ClickSearchCommand = new DelegateAsyncCommand(OnClickSearch);
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; }

        public DelegateAsyncCommand ClickSearchCommand { get; }

        [Time]
        public override async void Initialize(INavigationParameters parameters)
        {
            if (MatchItemsSource == null)
            {
                await LoadData(() => LoadMatches(DateRange.FromYesterdayUntilNow())).ConfigureAwait(false);
            }
            else
            {
                await LoadData(() => LoadMatches(selectedDateRange, true)).ConfigureAwait(false);
            }
        }

        [Time]
        public override async void OnResume()
        {
            Profiler.Start("ScoresViewModel.OnResume");

            if (SelectedDate != DateTime.Today)
            {
                await NavigateToHome().ConfigureAwait(false);
            }

            await LoadData(() => LoadMatches(selectedDateRange, true)).ConfigureAwait(false);

            OnInitialized();
        }

        protected override void OnInitialized()
        {
            cancellationTokenSource = new CancellationTokenSource();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventAggregator
                .GetEvent<DateBarItemSelectedEvent>()
                .Subscribe(OnDateBarItemSelected, true);

            EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Subscribe(OnReceivedMatchEvent, true);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Subscribe(OnReceivedTeamStatistic, true);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            UnsubscribeAllEvents();

            cancellationTokenSource?.Cancel();
        }

        private void UnsubscribeAllEvents()
        {
            EventAggregator
                .GetEvent<DateBarItemSelectedEvent>()
                .Unsubscribe(OnDateBarItemSelected);

            EventAggregator
               .GetEvent<MatchEventPubSubEvent>()
               .Unsubscribe(OnReceivedMatchEvent);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Unsubscribe(OnReceivedTeamStatistic);
        }

        private async Task OnRefresh()
        {
            Profiler.Start("ScoresViewModel.LoadMatches.PullDownToRefresh");

            await LoadData(() => LoadMatches(selectedDateRange, true), false)
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
                await LoggingService.LogErrorAsync(navigated.Exception)
                    .ConfigureAwait(false);
            }
        }

        private async Task OnClickSearch()
            => await NavigationService
                .NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true)
                .ConfigureAwait(false);

        private async void OnDateBarItemSelected(DateRange dateRange)
        {
            Profiler.Start("ScoresViewModel.LoadMatches.SelectDate");
            await LoadData(() => LoadMatches(dateRange)).ConfigureAwait(false);
        }

        [Time]
        private async Task LoadMatches(DateRange dateRange, bool forceFetchNewData = false)
        {
            await Task.Run(() => UnsubscribeLiveMatchTimeChangeEvent(dateRange)).ConfigureAwait(false);

            if (IsLoading)
            {
                MatchItemsSource = EmptyMatchDataSource;
            }

            var matches = await matchService.GetMatches(
                    dateRange,
                    Settings.CurrentLanguage,
                    forceFetchNewData).ConfigureAwait(false);

            Device.BeginInvokeOnMainThread(() => MatchItemsSource = BuildMatchItemSource(matches));

            selectedDateRange = dateRange;
            IsRefreshing = false;

            Profiler.Stop("ScoresViewModel.LoadMatches.PullDownToRefresh");
            Profiler.Start("ScoresView.Render");

            Debug.WriteLine($"Number of matches: {matches.Count()}");
        }

        private void UnsubscribeLiveMatchTimeChangeEvent(DateRange dateRange)
        {
            // TODO: Enhance later - Call Dispose() for closing Subscriber Match Time Event

            if ((dateRange.From == DateTime.Today
                 || dateRange.From == DateTimeExtension.Yesterday().Date)
                && MatchItemsSource?.Count > 0)
            {
                var liveMatchViewModels
                    = MatchItemsSource
                        .SelectMany(group => group)
                        .Where(matchItem => matchItem.Match.EventStatus?.IsLive == true);

                // experiment parallel
                Parallel.ForEach(liveMatchViewModels, (viewModel, _)
                    => viewModel.UnsubscribeMatchTimeChangeEvent());

                //liveMatchViewModels.ToList().ForEach(m => m.UnsubscribeMatchTimeChangeEvent());
            }
        }

        private ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels
                = matches.Select(
                    match => new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator));

            return new ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                matchItemViewModels
                    .GroupBy(item => new GroupMatchViewModel(item.Match)).ToList());
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