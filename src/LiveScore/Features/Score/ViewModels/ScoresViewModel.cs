using System;
using System.Collections.Generic;
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
            Profiler.Start(GetType().Name + ".LoadMatches.Home");

            SelectedDate = DateTime.Today;

            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());

            var hubService = DependencyResolver.Resolve<IHubService>(CurrentSportId.ToString());
            matchHubConnection = hubService.BuildMatchEventHubConnection();
            teamHubConnection = hubService.BuildTeamStatisticHubConnection();

            RefreshCommand = new DelegateAsyncCommand(OnRefreshCommand);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTappedMatchCommand);
            ClickSearchCommand = new DelegateAsyncCommand(OnClickSearchCommandExecuted);
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public IList<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; }

        public DelegateAsyncCommand ClickSearchCommand { get; }

        public override async void OnResume()
        {
            if (SelectedDate != DateTime.Today)
            {
                await NavigateToHome();
            }

            await LoadData(() => LoadMatches(selectedDateRange, true), false);

            Initialize();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
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

        private async Task OnRefreshCommand()
        {
            Profiler.Start(GetType().Name + ".LoadMatches.PullDownToRefresh");

            await LoadData(() => LoadMatches(selectedDateRange, true), false);
        }

        private async Task OnTappedMatchCommand(MatchViewModel matchItem)
        {
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

        private async Task OnClickSearchCommandExecuted()
        {
            await NavigationService.NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true);
        }

#pragma warning disable S3168 // "async" methods should not return "void"

        private async void OnDateBarItemSelected(DateRange dateRange)
        {
            Profiler.Start(GetType().Name + ".LoadMatches.SelectDate");
            await LoadData(() => LoadMatches(dateRange));
        }

#pragma warning restore S3168 // "async" methods should not return "void"

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
            Profiler.Stop(GetType().Name + ".LoadMatches.Home");
            Profiler.Stop(GetType().Name + ".LoadMatches.PullDownToRefresh");
            Profiler.Stop(GetType().Name + ".LoadMatches.SelectDate");
        }

        private IList<IGrouping<GroupMatchViewModel, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match => new MatchViewModel(match, DependencyResolver, CurrentSportId));

            return matchItemViewModels
                .GroupBy(item => new GroupMatchViewModel(item.Match.LeagueId, item.Match.LeagueName, item.Match.EventDate))
                .ToList();
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