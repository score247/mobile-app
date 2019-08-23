[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Helpers;
    using LiveScore.Core;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

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

        public IList<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; }

        public DelegateAsyncCommand ClickSearchCommand { get; }

        [Time]
        public override async void OnResume()
        {
            Profiler.Start("ScoresViewModel.OnResume");

            if (SelectedDate != DateTime.Today)
            {
                await NavigateToHome();
            }

            await LoadData(() => LoadMatches(selectedDateRange, true), false);

            OnInitialized();
        }

        [Time]
        public override async void Initialize(INavigationParameters parameters)
        {
            if (MatchItemsSource == null)
            {
                await LoadData(() => LoadMatches(DateRange.FromYesterdayUntilNow()));
            }
            else
            {
                await LoadData(() => LoadMatches(selectedDateRange, true), false);
            }
        }

        [Time]
        protected override void OnInitialized()
        {
            cancellationTokenSource = new CancellationTokenSource();

            EventAggregator
                .GetEvent<DateBarItemSelectedEvent>()
                .Subscribe(OnDateBarItemSelected, true);

            EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Subscribe(OnReceivedMatchEvent, true);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Subscribe(OnTeamStatisticChanged, true);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            EventAggregator
                .GetEvent<DateBarItemSelectedEvent>()
                .Unsubscribe(OnDateBarItemSelected);

            EventAggregator
               .GetEvent<MatchEventPubSubEvent>()
               .Unsubscribe(OnReceivedMatchEvent);

            cancellationTokenSource?.Cancel();
        }

        private async Task OnRefresh()
        {
            Profiler.Start("ScoresViewModel.LoadMatches.PullDownToRefresh");

            await LoadData(() => LoadMatches(selectedDateRange, true), false);
        }

        private async Task OnTapMatch(MatchViewModel matchItem)
        {
            // TODO: Change to use IAutoInitialize for parameters followed by new release of prism
            // https://github.com/PrismLibrary/Prism/releases
            var parameters = new NavigationParameters
            {
                { "Match", matchItem.Match }
            };

            var navigated = await NavigationService.NavigateAsync("MatchDetailView" + CurrentSportId, parameters);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception);
            }
        }

        private async Task OnClickSearch()
            => await NavigationService.NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true);

        private async void OnDateBarItemSelected(DateRange dateRange)
        {
            Profiler.Start("ScoresViewModel.LoadMatches.SelectDate");
            await LoadData(() => LoadMatches(dateRange));
        }

        [Time]
        private async Task LoadMatches(
            DateRange dateRange,
            bool forceFetchNewData = false)
        {
            if (IsLoading)
            {
                MatchItemsSource?.Clear();
            }

            var matches = await matchService.GetMatches(
                    dateRange,
                    SettingsService.Language,
                    forceFetchNewData);

            Device.BeginInvokeOnMainThread(() => MatchItemsSource = BuildMatchItemSource(matches));

            selectedDateRange = dateRange;
            IsRefreshing = false;

            Debug.WriteLine($"{GetType().Name}.Matches-DateRange:{dateRange.ToString()}: {matches.Count()}");
        }

        private IList<IGrouping<GroupMatchViewModel, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match => new MatchViewModel(match, DependencyResolver, CurrentSportId));

            return matchItemViewModels
                .GroupBy(item => new GroupMatchViewModel(item.Match.LeagueId, item.Match.LeagueName, item.Match.EventDate))
                .ToList();
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

        internal void OnTeamStatisticChanged(ITeamStatisticsMessage payload)
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