using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scores.Tests")]

namespace LiveScore.Score.ViewModels
{
    using Common.Extensions;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Navigation;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xamarin.Forms;

#pragma warning disable S2931 // Classes with "IDisposable" members should implement "IDisposable"

    public class ScoresViewModel : ViewModelBase
#pragma warning restore S2931 // Classes with "IDisposable" members should implement "IDisposable"
    {
        private const int HubKeepAliveInterval = 30;
        private readonly IMatchService matchService;
        private readonly HubConnection matchHubConnection;
        private readonly HubConnection teamHubConnection;
        private DateRange selectedDateRange;
        private CancellationTokenSource cancellationTokenSource;
        private readonly IMatchStatusConverter matchStatusConverter;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = DateTime.Today;

            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchStatusConverter = DependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());

            var hubService = DependencyResolver.Resolve<IHubService>(CurrentSportId.ToString());
            matchHubConnection = hubService.BuildMatchEventHubConnection();
            teamHubConnection = hubService.BuildTeamStatisticHubConnection();

            RefreshCommand = new DelegateCommand(OnRefreshCommand);
            TappedMatchCommand = new DelegateCommand<MatchViewModel>(OnTappedMatchCommand);
            ClickSearchCommand = new DelegateCommand(OnClickSearchCommandExecuted);
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateCommand RefreshCommand { get; }

        public DelegateCommand<MatchViewModel> TappedMatchCommand { get; }

        public DelegateCommand ClickSearchCommand { get; }

        public override void OnResume()
        {
            if (SelectedDate != DateTime.Today)
            {
                Device.BeginInvokeOnMainThread(async () => await NavigateToHome());
            }

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
            => Device.BeginInvokeOnMainThread(async () => await LoadData(() => LoadMatches(dateRange)));

        private async Task LoadMatches(
            DateRange dateRange,
            bool forceFetchNewData = false)
        {
            if (IsLoading)
            {
                MatchItemsSource?.Clear();
            }

            var matches = await matchService.GetMatches(
                    dateRange ?? DateRange.FromYesterdayUntilNow(),
                    SettingsService.Language,
                    forceFetchNewData);

            MatchItemsSource = BuildMatchItemSource(matches);

            selectedDateRange = dateRange;
            IsRefreshing = false;
        }

        private ObservableCollection<IGrouping<dynamic, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(
                    match => new MatchViewModel(match, matchHubConnection, matchStatusConverter, CurrentSportId));

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