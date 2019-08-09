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
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

#pragma warning disable S2931 // Classes with "IDisposable" members should implement "IDisposable"

    public class ScoresViewModel : ViewModelBase
#pragma warning restore S2931 // Classes with "IDisposable" members should implement "IDisposable"
    {
        private const int HubKeepAliveInterval = 30;
        private readonly IMatchService MatchService;
        private readonly HubConnection matchHubConnection;
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
            MatchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchHubConnection = DependencyResolver
                .Resolve<IHubService>(CurrentSportId.ToString())
                .BuildMatchEventHubConnection();
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadMatches(selectedDateRange, true), false));

            matchStatusConverter = DependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, MatchViewModel>> MatchItemsSource { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand
            => new DelegateAsyncCommand<MatchViewModel>(OnTappedMatchCommand);

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

        public DelegateAsyncCommand ClickSearchCommand => new DelegateAsyncCommand(OnClickSearchCommandExecuted);

        private async Task OnClickSearchCommandExecuted()
        {
            await NavigationService.NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true);
        }

        public override async void OnResume()
        {
            if (SelectedDate != DateTime.Today)
            {
                await NavigateToHome();
            }

            Initialize();
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (MatchItemsSource == null)
            {
                await LoadData(() => LoadMatches(DateRange.FromYesterdayUntilNow()));
            }
        }

        protected override async void Initialize()
        {
            try
            {
                cancellationTokenSource = new CancellationTokenSource();

                EventAggregator
                  .GetEvent<DateBarItemSelectedEvent>()
                  .Subscribe(OnDateBarItemSelected);

                MatchService.SubscribeMatchEvent(matchHubConnection, OnMatchesChanged);

                await matchHubConnection.StartWithKeepAlive(TimeSpan.FromSeconds(HubKeepAliveInterval), cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        protected override void Clean()
        {
            base.Clean();

            EventAggregator
                 .GetEvent<DateBarItemSelectedEvent>()
                 .Unsubscribe(OnDateBarItemSelected);

            cancellationTokenSource?.Dispose();
        }

#pragma warning disable S3168 // "async" methods should not return "void"

        private async void OnDateBarItemSelected(DateRange dateRange) => await LoadData(() => LoadMatches(dateRange));

#pragma warning restore S3168 // "async" methods should not return "void"

        private async Task LoadMatches(
            DateRange dateRange,
            bool forceFetchNewData = false)
        {
            if (IsLoading)
            {
                MatchItemsSource?.Clear();
            }

            var matches = await MatchService.GetMatches(
                    SettingsService.UserSettings,
                    dateRange ?? DateRange.FromYesterdayUntilNow(),
                    forceFetchNewData);

            MatchItemsSource = BuildMatchItemSource(matches);

            selectedDateRange = dateRange;
            IsRefreshing = false;
        }

        private ObservableCollection<IGrouping<dynamic, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(
                    match => new MatchViewModel(match, NavigationService, DependencyResolver, EventAggregator, matchHubConnection, matchStatusConverter));

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
                matchItem.Match.MatchResult = matchEvent.MatchResult;
                matchItem.Match.LatestTimeline = matchEvent.Timeline;
            }
        }
    }
}