using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Helpers;
using LiveScore.Core;
using LiveScore.Core.Controls.DateBar.Events;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Score.ViewModels
{
    public class ScoresViewModel : ViewModelBase

    {
        private const int HubKeepAliveInterval = 30;
        private readonly IMatchService matchService;
        private readonly HubConnection matchHubConnection;
        private readonly HubConnection teamHubConnection;
        private DateRange selectedDateRange;
        private CancellationTokenSource cancellationTokenSource;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            Profiler.Start(this.GetType().Name + ".LoadMatches.Home");

            SelectedDate = DateTime.Today;

            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            var hubService = DependencyResolver.Resolve<IHubService>(CurrentSportId.ToString());
            matchHubConnection = hubService.BuildMatchEventHubConnection();
            teamHubConnection = hubService.BuildTeamStatisticHubConnection();

            RefreshCommand = new DelegateCommand(OnRefreshCommand);
            TappedMatchCommand = new DelegateCommand<MatchViewModel>(OnTappedMatchCommand);
            ClickSearchCommand = new DelegateCommand(OnClickSearchCommandExecuted);
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public IList<IGrouping<dynamic, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateCommand RefreshCommand { get; }

        public DelegateCommand<MatchViewModel> TappedMatchCommand { get; }

        public DelegateCommand ClickSearchCommand { get; }

        public override void OnResume()
        {
            if (SelectedDate != DateTime.Today)
            {
                Device.BeginInvokeOnMainThread(async () => await NavigateToHome());
            }

            Device.BeginInvokeOnMainThread(async () =>
                await LoadData(() => LoadMatches(selectedDateRange, true), false));

            Initialize();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (MatchItemsSource == null)
            {
                Device.BeginInvokeOnMainThread(async ()
                    => await LoadData(() => LoadMatches(DateRange.FromYesterdayUntilNow())));
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                    await LoadData(() => LoadMatches(selectedDateRange, true), false));
            }
        }

        protected override void Initialize()
        {
            cancellationTokenSource = new CancellationTokenSource();

            EventAggregator
              .GetEvent<DateBarItemSelectedEvent>()
              .Subscribe(OnDateBarItemSelected);

            matchService.SubscribeMatchEvent(matchHubConnection, OnMatchesChanged);
            matchService.SubscribeTeamStatistic(teamHubConnection, OnTeamStatisticChanged);

            Device.BeginInvokeOnMainThread(async () =>
                await teamHubConnection.StartWithKeepAlive(TimeSpan.FromSeconds(HubKeepAliveInterval), LoggingService, cancellationTokenSource.Token));

            Device.BeginInvokeOnMainThread(async () =>
                await matchHubConnection.StartWithKeepAlive(TimeSpan.FromSeconds(HubKeepAliveInterval), LoggingService, cancellationTokenSource.Token));
        }

        protected override void Clean()
        {
            base.Clean();

            EventAggregator
                 .GetEvent<DateBarItemSelectedEvent>()
                 .Unsubscribe(OnDateBarItemSelected);

            cancellationTokenSource?.Cancel();
        }

        private void OnRefreshCommand()
        {
            Profiler.Start(this.GetType().Name + ".LoadMatches.PullDownToRefresh");

            Device.BeginInvokeOnMainThread(async () =>
                await LoadData(() => LoadMatches(selectedDateRange, true), false));
        }

        private void OnTappedMatchCommand(MatchViewModel matchItem)
        {
            var parameters = new NavigationParameters
            {
                { "Match", matchItem.Match }
            };

            Device.BeginInvokeOnMainThread(async () =>
            {
                var navigated = await NavigationService.NavigateAsync("MatchDetailView" + CurrentSportId, parameters);

                if (!navigated.Success)
                {
                    await LoggingService.LogErrorAsync(navigated.Exception);
                }
            });
        }

        private void OnClickSearchCommandExecuted()
        {
            Device.BeginInvokeOnMainThread(async () =>
                await NavigationService.NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true));
        }

        private void OnDateBarItemSelected(DateRange dateRange)
        {
            Profiler.Start(this.GetType().Name + ".LoadMatches.SelectDate");
            Device.BeginInvokeOnMainThread(async () => await LoadData(() => LoadMatches(dateRange)));
        }

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

            MatchItemsSource = BuildMatchItemSource(matches.ToList());

            selectedDateRange = dateRange;
            IsRefreshing = false;

            Profiler.Stop("ScoresViewModel.LoadMatches.PullDownToRefresh");
            Debug.WriteLine($"{this.GetType().Name}.Matches-DateRange:{dateRange.ToString()}: {matches.Count()}");
        }

        private IList<IGrouping<dynamic, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            // TODO: Enhance later - Call Dispose() for closing Subscriber Match Time Event 
            if (MatchItemsSource?.Any() == true)
            {
                var liveMatchViewModels = MatchItemsSource.SelectMany(g => g).Where(m => m.Match.MatchResult.EventStatus?.IsLive == true);
                liveMatchViewModels.ToList().ForEach(m => m.Dispose());
            }

            var matchItemViewModels = matches.Select(
                    match => new MatchViewModel(match, DependencyResolver, CurrentSportId));

            return new ObservableCollection<IGrouping<dynamic, MatchViewModel>>(matchItemViewModels.GroupBy(item
                => new { item.Match.League.Id, item.Match.League.Name, item.Match.EventDate.LocalDateTime.Day, item.Match.EventDate.LocalDateTime.Month, item.Match.EventDate.LocalDateTime.Year }));
        }

        internal void OnMatchesChanged(byte sportId, IMatchEvent matchEvent)
        {
            if (sportId != CurrentSportId)
            {
                return;
            }

            var matchItem = MatchItemsSource
               .SelectMany(group => group)
               .FirstOrDefault(m => m.Match.Id == matchEvent.MatchId);

            if (matchItem?.Match != null)
            {
                matchItem.OnReceivedMatchEvent(matchEvent);
            }
        }

        internal void OnTeamStatisticChanged(byte sportId, string matchId, bool isHome, ITeamStatistic teamStats)
        {
            if (sportId != CurrentSportId)
            {
                return;
            }

            var matchItem = MatchItemsSource
              .SelectMany(group => group)
              .FirstOrDefault(m => m.Match.Id == matchId);

            matchItem.OnReceivedTeamStatistic(isHome, teamStats);
        }
    }
}