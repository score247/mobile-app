using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.Helpers;
using LiveScore.Core;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Features.Score.Extensions;
using MethodTimer;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Device = Xamarin.Forms.Device;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Features.Score.ViewModels
{
    public class ScoreItemViewModel : ViewModelBase
    {
        protected readonly IMatchStatusConverter matchStatusConverter;
        protected readonly IMatchMinuteConverter matchMinuteConverter;
        protected readonly Func<string, string> buildFlagUrlFunc;
        protected readonly IMatchService MatchService;
        private const int DefaultFirstLoadMatchItemCount = 5;
        private const int DefaultLoadingMatchItemCountOnScrolling = 8;
        private readonly bool isTodayOrYesterday;

        [Time]
        public ScoreItemViewModel(
            DateTime selectedDate,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = selectedDate;

            MatchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchStatusConverter = DependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
            matchMinuteConverter = DependencyResolver.Resolve<IMatchMinuteConverter>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            isTodayOrYesterday = SelectedDate == DateTime.Today || SelectedDate == DateTime.Today.AddDays(-1);
            MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>();
            RemainingMatchItemSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>();

            SubscribeEvents();
            InitializeCommand();
        }

        public bool FirstLoad { get; private set; } = true;

        public DateTime SelectedDate { get; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; protected set; }

        public ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> RemainingMatchItemSource { get; protected set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; private set; }

        public DelegateCommand LoadMoreCommand { get; private set; }

        public override void Destroy()
        {
            base.Destroy();

            UnsubscribeAllEvents();
        }

        public override void OnResumeWhenNetworkOK()
        {
            Profiler.Start("ScoreItemViewModel.OnResume");

            if (isTodayOrYesterday)
            {
                Task.Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false));
            }
        }

        public override Task OnNetworkReconnected()
        {
            return Task.Run(() => LoadDataAsync(() => LoadMatchesAsync(true)));
        }

        [Time]
        public override void OnAppearing()
        {
            base.OnAppearing();

            CheckNetworkAndRunAction(() =>
            {
                if (FirstLoad)
                {
                    FirstLoad = false;

                    LoadDataAsync(() => LoadMatchesAsync()).ConfigureAwait(false);
                }
                else
                {
                    if (isTodayOrYesterday)
                    {
                        Task.Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false));
                    }
                }
            });
        }

        private void CheckNetworkAndRunAction(Action action)
        {
            if (networkConnectionManager.IsConnectionOK())
            {
                action();
            }
            else
            {
                IsRefreshing = false;
                networkConnectionManager.PublishNetworkConnectionEvent();
            }
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

        internal void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            if (payload?.SportId != CurrentSportId)
            {
                return;
            }

            MatchItemsSource?.UpdateMatchItemEvent(payload.MatchEvent);
            RemainingMatchItemSource?.UpdateMatchItemEvent(payload.MatchEvent);
        }

        internal void OnReceivedTeamStatistic(ITeamStatisticsMessage payload)
        {
            if (payload.SportId != CurrentSportId)
            {
                return;
            }

            MatchItemsSource?.UpdateMatchItemStatistics(payload.MatchId, payload.IsHome, payload.TeamStatistic);
            RemainingMatchItemSource?.UpdateMatchItemStatistics(payload.MatchId, payload.IsHome, payload.TeamStatistic);
        }

        private void InitializeCommand()
        {
            RefreshCommand = new DelegateAsyncCommand(OnRefreshAsync);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatchAsync);
            LoadMoreCommand = new DelegateCommand(OnLoadMore);
        }

        private async Task OnRefreshAsync()
        {
            Profiler.Start("ScoreItemViewModel.LoadMatches.PullDownToRefresh");
            if (networkConnectionManager.IsConnectionNotOK())
            {
                IsRefreshing = false;
                networkConnectionManager.PublishNetworkConnectionEvent();
                return;
            }

            await Task.Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false)).ConfigureAwait(false);
        }

        private async Task OnTapMatchAsync(MatchViewModel matchItem)
        {
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

        private void OnLoadMore()
        {
            if (RemainingMatchItemSource?.Any() != true)
            {
                return;
            }

            var matchItems = RemainingMatchItemSource.Take(DefaultLoadingMatchItemCountOnScrolling);

            RemainingMatchItemSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                RemainingMatchItemSource.Skip(DefaultLoadingMatchItemCountOnScrolling));

            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var matchItem in matchItems)
                {
                    MatchItemsSource.Add(matchItem);
                }
            });
        }

        protected virtual async Task LoadMatchesAsync(bool getLatestData = false)
        {
            var matches = (await LoadMatchesFromServiceAsync(getLatestData))?.ToList();

            if (matches?.Any() != true)
            {
                if (MatchItemsSource?.Any() != true)
                {
                    HasData = false;
                }

                return;
            }

            HasData = true;
            InitializeMatchItems(matches);
        }

        protected virtual async Task UpdateMatchesAsync(bool getLatestData = false)
        {
            var matches = (await LoadMatchesFromServiceAsync(getLatestData))?.ToList();

            if (matches?.Any() != true)
            {
                HasData = false;
                MatchItemsSource?.Clear();
                RemainingMatchItemSource?.Clear();
                IsRefreshing = false;
                return;
            }

            UpdateMatchItems(matches);
            IsRefreshing = false;
            HasData = true;
        }

        protected virtual async Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync(bool getLatestData)
        {
            return await MatchService
                .GetMatchesByDate(SelectedDate, CurrentLanguage, getLatestData)
                .ConfigureAwait(false);
        }

        protected virtual void UpdateMatchItems(List<IMatch> matches)
        {
            try
            {
                if (MatchItemsSource.Count == 0 && matches.Count > 0)
                {
                    InitializeMatchItems(matches);
                    return;
                }

                var loadedMatches = matches.Where(m => MatchItemsSource.Any(l => l.Any(lm => lm.Match.Id == m.Id))).ToList();
                Device.BeginInvokeOnMainThread(() =>
                    MatchItemsSource.UpdateMatchItems(
                        loadedMatches, matchStatusConverter, matchMinuteConverter, EventAggregator, buildFlagUrlFunc));

                var remainingMatches = matches.Except(loadedMatches);
                RemainingMatchItemSource.UpdateMatchItems(
                    remainingMatches, matchStatusConverter, matchMinuteConverter, EventAggregator, buildFlagUrlFunc);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex);
            }
        }

        [Time]
        protected virtual void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match =>
                new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator));

            var matchItems = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc)).ToList();
            var loadItems = matchItems.Take(DefaultFirstLoadMatchItemCount);
            RemainingMatchItemSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                matchItems.Skip(DefaultFirstLoadMatchItemCount));

            Device.BeginInvokeOnMainThread(()
                => MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(loadItems));
        }
    }
}