using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Helpers;
using LiveScore.Core;
using LiveScore.Core.Controls.DateBar.Events;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using MethodTimer;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Score.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private readonly IMatchService matchService;
        private DateRange selectedDateRange;
        private CancellationTokenSource cancellationTokenSource;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            Profiler.Start("ScoresViewModel.LoadMatches.Home");

            SelectedDate = DateTime.Today;

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

            var navigated = await NavigationService.NavigateAsync("MatchDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        private async Task OnClickSearch()
            => await NavigationService.NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true).ConfigureAwait(false);

        private async void OnDateBarItemSelected(DateRange dateRange)
        {
            Profiler.Start("ScoresViewModel.LoadMatches.SelectDate");
            await LoadData(() => LoadMatches(dateRange)).ConfigureAwait(false);
        }

        [Time]
        private async Task LoadMatches(
            DateRange dateRange,
            bool forceFetchNewData = false)
        {
            if (IsLoading)
            {
                MatchItemsSource
                    = new ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                        new List<IGrouping<GroupMatchViewModel, MatchViewModel>>());
            }

            var matches = await matchService.GetMatches(
                    dateRange,
                    SettingsService.Language,
                    forceFetchNewData).ConfigureAwait(false);

            Device.BeginInvokeOnMainThread(() => MatchItemsSource = BuildMatchItemSource(matches));

            selectedDateRange = dateRange;
            IsRefreshing = false;

            Debug.WriteLine($"{GetType().Name}.Matches-DateRange:{dateRange.ToString()}: {matches.Count()}");
        }

        private ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match => new MatchViewModel(match, DependencyResolver, CurrentSportId));

            return new ReadOnlyCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                matchItemViewModels.GroupBy(item
                => new GroupMatchViewModel(item.Match.LeagueId,
                                           item.Match.LeagueName,
                                           item.Match.EventDate)).ToList());
        }

        internal void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            if (payload?.SportId == CurrentSportId)
            {
                var matchItem = MatchItemsSource
                  .SelectMany(group => group)
                  .FirstOrDefault(m => m.Match.Id == payload.MatchEvent.MatchId);

                if (matchItem?.Match != null)
                {
                    matchItem.OnReceivedMatchEvent(payload.MatchEvent);
                }
            }
        }

        internal void OnReceivedTeamStatistic(ITeamStatisticsMessage payload)
        {
            if (payload.SportId == CurrentSportId)
            {
                var matchItem = MatchItemsSource
                     .SelectMany(group => group)
                     .FirstOrDefault(m => m.Match.Id == payload.MatchId);

                matchItem.OnReceivedTeamStatistic(payload.IsHome, payload.TeamStatistic);
            }
        }
    }
}