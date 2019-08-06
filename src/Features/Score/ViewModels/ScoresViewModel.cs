using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scores.Tests")]

namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;

#pragma warning disable S2931 // Classes with "IDisposable" members should implement "IDisposable"

    public class ScoresViewModel : ViewModelBase
#pragma warning restore S2931 // Classes with "IDisposable" members should implement "IDisposable"
    {
        private const int HubKeepAliveInterval = 30;
        private readonly IMatchService MatchService;
        private readonly HubConnection matchHubConnection;
        private DateRange selectedDateRange;
        private CancellationTokenSource cancellationTokenSource;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            IHubService hubService)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = DateTime.Today;
            MatchService = DependencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.DisplayName);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadMatches(selectedDateRange, true), false));
            matchHubConnection = hubService.BuildMatchHubConnection();
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, MatchViewModel>> MatchItemSource { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand
            => new DelegateAsyncCommand<MatchViewModel>(OnTappedMatchCommand);

        private async Task OnTappedMatchCommand(MatchViewModel matchItem)
        {
            var parameters = new NavigationParameters
            {
                { "Match", matchItem.Match }
            };

            var navigated = await NavigationService.NavigateAsync("MatchDetailView" + SettingsService.CurrentSportType.Value, parameters);

            if(!navigated.Success)
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
            if (MatchItemSource == null)
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

                MatchService.SubscribeMatches(matchHubConnection, OnMatchesChanged);

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
                MatchItemSource?.Clear();
            }

            var matches = await MatchService.GetMatches(
                    SettingsService.UserSettings,
                    dateRange ?? DateRange.FromYesterdayUntilNow(),
                    forceFetchNewData);

            MatchItemSource = BuildMatchItemSource(matches);

            selectedDateRange = dateRange;
            IsRefreshing = false;
        }

        private ObservableCollection<IGrouping<dynamic, MatchViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(
                    match => new MatchViewModel(match, NavigationService, DependencyResolver, EventAggregator, matchHubConnection));

            return new ObservableCollection<IGrouping<dynamic, MatchViewModel>>(matchItemViewModels.GroupBy(item
                => new { item.Match.League.Id, item.Match.League.Name, item.Match.EventDate.Day, item.Match.EventDate.Month, item.Match.EventDate.Year }));
        }

        internal void OnMatchesChanged(byte sportId, Dictionary<string, MatchPushEvent> matchPayloads)
        {
            if (sportId != SettingsService.CurrentSportType.Value)
            {
                return;
            }

            foreach (var matchPayload in matchPayloads)
            {
                var matchItem = MatchItemSource
                   .SelectMany(group => group)
                   .FirstOrDefault(m => m.Match.Id == matchPayload.Key);

                if (matchItem?.Match != null)
                {
                    matchItem.Match.MatchResult = matchPayload.Value.MatchResult;
                    matchItem.Match.LatestTimeline = matchPayload.Value.TimeLines.LastOrDefault();
                    matchItem.BuildMatchStatus();
                }
            }
        }
    }
}