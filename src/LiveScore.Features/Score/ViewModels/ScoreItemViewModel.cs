using System.Text.RegularExpressions;
using ImTools;
using LiveScore.Core.Models.Matches;
using SharpRaven.Data.Context;
using Xamarin.Forms.Internals;
using Device = Xamarin.Forms.Device;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Features.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Helpers;
    using Core;
    using Core.Converters;
    using Core.PubSubEvents.Matches;
    using Core.PubSubEvents.Teams;
    using Core.Services;
    using LiveScore.Core.ViewModels;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoreItemViewModel : ViewModelBase
    {
        private static readonly ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> EmptyMatchDataSource =
            new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(Enumerable.Empty<IGrouping<GroupMatchViewModel, MatchViewModel>>().ToList());

        private readonly IMatchService matchService;
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly IMatchMinuteConverter matchMinuteConverter;

        [Time]
        public ScoreItemViewModel(
            DateTime selectedDate,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            bool isLive = false,
            bool isCalendar = false)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = selectedDate;
            IsLive = isLive;
            IsCalendar = isCalendar;
            matchStatusConverter = dependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
            matchMinuteConverter = dependencyResolver.Resolve<IMatchMinuteConverter>(CurrentSportId.ToString());
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());

            SubscribeEvents();
            InitializeCommand();
        }

        public bool FirstLoad { get; private set; } = true;

        public DateTime SelectedDate { get; }

        public bool IsLive { get; }

        public bool IsCalendar { get; }

        public bool IsNormalDate => !IsLive && !IsCalendar;

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

            Task.Run(async ()
                => await LoadData(() => UpdateMatches(true), false));
        }

        [Time]
        public override async void OnAppearing()
        {
            if (FirstLoad)
            {
                FirstLoad = false;

                await LoadData(() => LoadMatches(SelectedDate));
            }
            else
            {
                await Task.Run(async () => await LoadData(() => UpdateMatches(), false));
            }
        }

        private void InitializeCommand()
        {
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatch);
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

        private async Task OnRefresh()
        {
            Profiler.Start("ScoreItemViewModel.LoadMatches.PullDownToRefresh");

            await Task.Run(async () => await LoadData(() => UpdateMatches(true), false));
        }

        private async Task OnTapMatch(MatchViewModel matchItem)
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

        [Time]
        private async Task LoadMatches(DateTime date, bool forceFetchNewData = false)
        {
            if (IsBusy)
            {
                MatchItemsSource = EmptyMatchDataSource;
            }

            var matches = await matchService.GetMatchesByDate(
                date,
                CurrentLanguage,
                forceFetchNewData).ConfigureAwait(false);

            var matchItemViewModels = matches
                .Select(match => new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator))
                .ToList();

            var groups = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match));

            MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(groups);
            IsRefreshing = false;

            Profiler.Start("ScoresView.Render");
            Debug.WriteLine($"Number of groups: {groups.Count()}");
            Debug.WriteLine($"Number of matches: {matchItemViewModels.Count}");

            Profiler.Stop("ScoreItemViewModel.LoadMatches.PullDownToRefresh");
        }

        private async Task UpdateMatches(bool forceFetchNewData = false)
        {
            var matches = await matchService.GetMatchesByDate(
                SelectedDate,
                CurrentLanguage,
                forceFetchNewData);

            var matchViewModels = MatchItemsSource?.SelectMany(g => g);

            foreach (var match in matches)
            {
                var matchViewModel = matchViewModels?.FirstOrDefault(m => m.Match.Id == match.Id);

                if (matchViewModel == null)
                {
                    AddNewMatchToItemSource(match);

                    continue;
                }

                if (match.ModifiedTime > matchViewModel.Match.ModifiedTime)
                {
                    matchViewModel.BuildMatch(match);
                }
            }

            Device.BeginInvokeOnMainThread(() => IsRefreshing = false);
        }

        private void AddNewMatchToItemSource(IMatch match)
        {
            var newMatchViewModel = new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator);
            var currentGroupIndex = MatchItemsSource.IndexOf(g => g.Key.LeagueId == match.LeagueId);
            var currentMatchViewModels = new List<MatchViewModel>();

            if (currentGroupIndex >= 0)
            {
                currentMatchViewModels = MatchItemsSource[currentGroupIndex].ToList();
                currentMatchViewModels.Add(newMatchViewModel);

                MatchItemsSource[currentGroupIndex] = currentMatchViewModels
                    .OrderBy(m => m.Match.EventDate)
                    .GroupBy(item => new GroupMatchViewModel(item.Match))
                    .FirstOrDefault();
            }
            else
            {
                currentMatchViewModels.Add(newMatchViewModel);

                MatchItemsSource.Add(currentMatchViewModels
                   .OrderBy(m => m.Match.EventDate)
                   .GroupBy(item => new GroupMatchViewModel(item.Match))
                   .FirstOrDefault());
            }


        }
    }
}