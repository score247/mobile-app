using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
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
        private const int DefaultFirstLoadMatchItemCount = 5;
        private const int DefaultLoadingMatchItemCountOnScrolling = 16;

        protected readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        protected readonly IMatchMinuteBuilder matchMinuteBuilder;
        protected readonly IMatchService matchService;
        protected readonly Func<string, string> buildFlagUrlFunc;

        [Time]
        public ScoreItemViewModel(
            DateTime viewDate,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            ViewDate = viewDate;

            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            matchMinuteBuilder = DependencyResolver.Resolve<IMatchMinuteBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            RemainingMatchItemSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>();

            InitializeCommand();
        }

        public bool FirstLoad { get; private set; } = true;

        public DateTime ViewDate { get; }

        public bool IsActive { get; set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; protected set; }

        public ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> RemainingMatchItemSource { get; protected set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; private set; }

        public DelegateCommand LoadMoreCommand { get; private set; }

        public override Task OnNetworkReconnectedAsync()
            => Task.Run(() => LoadDataAsync(() => LoadMatchesAsync(true)));

        public override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();
            SubscribeEvents();

            if (ViewDate.IsTodayOrYesterday())
            {
                Task.Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false));
            }
        }

        public override void OnSleep() => UnsubscribeAllEvents();

        [Time]
        public override void OnAppearing()
        {
            base.OnAppearing();
            SubscribeEvents();

            networkConnectionManager
                .OnSuccessfulConnection(() =>
                {
                    if (FirstLoad)
                    {
                        FirstLoad = false;
                        LoadDataAsync(() => LoadMatchesAsync()).ConfigureAwait(false);
                    }
                    else
                    {
                        if (ViewDate.IsTodayOrYesterday())
                        {
                            Task.Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false));
                        }
                    }
                })
                .OnFailedConnection(() =>
                {
                    IsRefreshing = false;
                    networkConnectionManager.PublishNetworkConnectionEvent();
                });
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            UnsubscribeAllEvents();
        }

        private void SubscribeEvents()
        {
            if (ViewDate.IsTodayOrYesterday())
            {
                EventAggregator
                    .GetEvent<MatchEventPubSubEvent>()
                    .Subscribe(OnReceivedMatchEvent, true);

                EventAggregator
                    .GetEvent<TeamStatisticPubSubEvent>()
                    .Subscribe(OnReceivedTeamStatistic, true);
            }
        }

        private void UnsubscribeAllEvents()
        {
            if (ViewDate.IsTodayOrYesterday())
            {
                EventAggregator
                    .GetEvent<MatchEventPubSubEvent>()
                    .Unsubscribe(OnReceivedMatchEvent);

                EventAggregator
                    .GetEvent<TeamStatisticPubSubEvent>()
                    .Unsubscribe(OnReceivedTeamStatistic);
            }
        }

        [Time]
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

            MatchItemsSource?.UpdateMatchItemStatistics(
                payload.MatchId,
                payload.IsHome,
                payload.TeamStatistic);

            RemainingMatchItemSource?.UpdateMatchItemStatistics(
                payload.MatchId,
                payload.IsHome,
                payload.TeamStatistic);
        }

        private void InitializeCommand()
        {
            RefreshCommand = new DelegateAsyncCommand(OnRefreshAsync);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatchAsync);
            LoadMoreCommand = new DelegateCommand(OnLoadMore);
        }

        private async Task OnRefreshAsync()
        {
            if (networkConnectionManager.IsFailureConnection())
            {
                IsRefreshing = false;
                networkConnectionManager.PublishNetworkConnectionEvent();
                return;
            }

            await Task.Run(
                () => LoadDataAsync(() => UpdateMatchesAsync(true), false))
                .ConfigureAwait(false);
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
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        [Time]
        private void OnLoadMore()
        {
            if (RemainingMatchItemSource?.Any() != true)
            {
                IsLoadingSkeleton = false;
                return;
            }

            IsLoadingSkeleton = true;

            Task.Delay(800)
                .ContinueWith(_ =>
                {
                    var matchItems = RemainingMatchItemSource.Take(DefaultLoadingMatchItemCountOnScrolling);

                    RemainingMatchItemSource
                        = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(
                            RemainingMatchItemSource.Skip(DefaultLoadingMatchItemCountOnScrolling));

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MatchItemsSource.AddItems(matchItems);
                    });
                });
        }

        protected virtual async Task LoadMatchesAsync(bool getLatestData = false)
        {
            var matches = await LoadMatchesFromServiceAsync(getLatestData).ConfigureAwait(false);

            if (matches?.Any() != true)
            {
                HasData = false;
                Device.BeginInvokeOnMainThread(() => MatchItemsSource?.Clear());
                return;
            }

            HasData = true;
            InitializeMatchItems(matches);
        }

        protected virtual async Task UpdateMatchesAsync(bool getLatestData = false)
        {
            var matches = await LoadMatchesFromServiceAsync(getLatestData).ConfigureAwait(false);

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

        protected virtual Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync(bool getLatestData)
            => matchService.GetMatchesByDateAsync(ViewDate, CurrentLanguage, getLatestData);

        protected virtual void UpdateMatchItems(IEnumerable<IMatch> matches)
        {
            try
            {
                if (MatchItemsSource?.Count == 0 && matches?.Any() == true)
                {
                    InitializeMatchItems(matches);
                    return;
                }

                var loadedMatches = matches
                    .Where(m => MatchItemsSource.Any(l => l.Any(lm => lm.Match?.Id == m.Id)));

                Device.BeginInvokeOnMainThread(() =>
                    MatchItemsSource.UpdateMatchItems(
                        loadedMatches,
                        matchStatusBuilder,
                        matchMinuteBuilder,
                        EventAggregator,
                        buildFlagUrlFunc));

                var remainingMatches = matches.Except(loadedMatches);

                RemainingMatchItemSource?
                    .UpdateMatchItems(
                        remainingMatches,
                        matchStatusBuilder,
                        matchMinuteBuilder,
                        EventAggregator,
                        buildFlagUrlFunc);
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
            }
        }

        [Time]
        protected virtual void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match => new MatchViewModel(
                match,
                matchStatusBuilder,
                matchMinuteBuilder,
                EventAggregator));

            var matchItems
                = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc));

            var loadItems = matchItems.Take(DefaultFirstLoadMatchItemCount);

            RemainingMatchItemSource
                = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(matchItems.Skip(DefaultFirstLoadMatchItemCount));

            Device.BeginInvokeOnMainThread(() =>
            {
                MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(loadItems);
            });
        }
    }
}