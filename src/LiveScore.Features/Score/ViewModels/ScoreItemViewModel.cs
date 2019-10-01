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
using MethodTimer;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms.Internals;
using Device = Xamarin.Forms.Device;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Features.Score.ViewModels
{
    public class ScoreItemViewModel : ViewModelBase
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly IMatchMinuteConverter matchMinuteConverter;
        protected readonly Func<string, string> buildFlagUrlFunc;
        protected readonly IMatchService MatchService;

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
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(Constants.BuildFlagUrlFunctionName);

            MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>();

            SubscribeEvents();
            InitializeCommand();
        }

        public bool FirstLoad { get; private set; } = true;

        public DateTime SelectedDate { get; }

        public bool HasNoData { get; private set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> MatchItemsSource { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; private set; }

        public override void Destroy()
        {
            base.Destroy();

            UnsubscribeAllEvents();
        }

        [Time]
        public override void OnResume()
        {
            Profiler.Start("ScoreItemViewModel.OnResume");

            UpdateMatchesInBackgroundAsync().ConfigureAwait(false);
        }

        [Time]
        public override void OnAppearing()
        {
            if (FirstLoad)
            {
                FirstLoad = false;

                LoadDataAsync(() => LoadMatchesAsync()).ConfigureAwait(false);
            }
            else
            {
                UpdateMatchesInBackgroundAsync().ConfigureAwait(false);
            }
        }

        private async Task OnRefreshAsync()
        {
            Profiler.Start("ScoreItemViewModel.LoadMatches.PullDownToRefresh");

            await Task
                .Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false))
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
                await LoggingService.LogErrorAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        private void InitializeCommand()
        {
            RefreshCommand = new DelegateAsyncCommand(OnRefreshAsync);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatchAsync);
        }

        internal void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            if (payload?.SportId != CurrentSportId)
            {
                return;
            }

            var matchItem = MatchItemsSource?
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

            var matchItem = MatchItemsSource?
                .SelectMany(group => group)
                .FirstOrDefault(m => m.Match.Id == payload.MatchId);

            matchItem?.OnReceivedTeamStatistic(payload.IsHome, payload.TeamStatistic);
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

        protected virtual async Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync(DateTime date, bool getLatestData)
            => await MatchService
                .GetMatchesByDate(date, CurrentLanguage, getLatestData)
                .ConfigureAwait(false);

        [Time]
        private async Task LoadMatchesAsync(bool getLatestData = false)
        {
            var matches = (await LoadMatchesFromServiceAsync(SelectedDate, getLatestData)
                    .ConfigureAwait(false))
                    ?.ToList();

            if (HasNoMatchData(matches))
            {
                return;
            }

            InitMatchItemSource(matches);
        }

        private void InitMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches
                .Select(match => new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator));

            var groups = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc));

            Device
                .BeginInvokeOnMainThread(() => MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(groups));
        }

        private async Task UpdateMatchesInBackgroundAsync()
        {
            if (SelectedDate == DateTime.Today || SelectedDate == DateTime.Today.AddDays(-1))
            {
                await Task
                    .Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false))
                    .ConfigureAwait(false);
            }
        }

        private async Task UpdateMatchesAsync(bool getLatestData = false)
        {
            try
            {
                var matches = (await LoadMatchesFromServiceAsync(SelectedDate, getLatestData)
                        .ConfigureAwait(false))
                        ?.ToList();

                UpdateMatchItemSource(matches);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex).ConfigureAwait(false);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => IsRefreshing = false);
            }
        }

        protected virtual void UpdateMatchItemSource(List<IMatch> matches)
        {
            if (HasNoMatchData(matches))
            {
                MatchItemsSource.Clear();
                Device.BeginInvokeOnMainThread(() => IsRefreshing = false);

                return;
            }

            var matchViewModels = MatchItemsSource?.SelectMany(g => g).ToList();

            foreach (var match in matches)
            {
                var matchViewModel = matchViewModels?.FirstOrDefault(viewModel => viewModel.Match.Id == match.Id);

                if (matchViewModel == null)
                {
                    AddNewMatchToItemSource(match);

                    continue;
                }

                if (match.ModifiedTime > matchViewModel.Match.ModifiedTime)
                {
                    Device.BeginInvokeOnMainThread(() => matchViewModel.BuildMatch(match));
                }
            }
        }

        protected virtual void AddNewMatchToItemSource(IMatch newMatch)
        {
            var currentGroupIndex = MatchItemsSource.IndexOf(g => g.Key.LeagueId == newMatch.LeagueId);
            List<MatchViewModel> currentMatchViewModels;

            if (currentGroupIndex >= 0)
            {
                currentMatchViewModels = MatchItemsSource[currentGroupIndex].ToList();

                currentMatchViewModels
                    .Add(new MatchViewModel(newMatch, matchStatusConverter, matchMinuteConverter, EventAggregator));

                var group = currentMatchViewModels
                        .OrderBy(m => m.Match.EventDate)
                        .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                        .FirstOrDefault();

                Device.BeginInvokeOnMainThread(() => MatchItemsSource[currentGroupIndex] = group);
            }
            else
            {
                currentMatchViewModels = new List<MatchViewModel>
                {
                    new MatchViewModel(newMatch, matchStatusConverter, matchMinuteConverter, EventAggregator)
                };

                var group = currentMatchViewModels
                       .OrderBy(m => m.Match.EventDate)
                       .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                       .FirstOrDefault();

                // TODO: Should fix: This code does not move favorite/major leagues to top
                Device.BeginInvokeOnMainThread(() => MatchItemsSource.Add(group));
            }
        }

        private bool HasNoMatchData(IEnumerable<IMatch> matches)
        {
            HasNoData = matches?.Any() != true;

            return HasNoData;
        }
    }
}