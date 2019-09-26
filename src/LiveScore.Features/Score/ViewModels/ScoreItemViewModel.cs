﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchStatusConverter = DependencyResolver.Resolve<IMatchStatusConverter>(CurrentSportId.ToString());
            matchMinuteConverter = DependencyResolver.Resolve<IMatchMinuteConverter>(CurrentSportId.ToString());

            MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>();

            SubscribeEvents();
            InitializeCommand();
        }

        public bool FirstLoad { get; private set; } = true;

        public DateTime SelectedDate { get; }

        public bool IsLive { get; }

        public bool IsCalendar { get; }

        public bool IsNormalDate => !IsCalendar;

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

            UpdateMatchesForTodayAndLiveMatchesAsync(true).ConfigureAwait(false);
        }

        [Time]
        public override void OnAppearing()
        {
            if (FirstLoad)
            {
                FirstLoad = false;

                LoadDataAsync(() => LoadMatchesAsync(SelectedDate)).ConfigureAwait(false);
            }
            else
            {
                UpdateMatchesForTodayAndLiveMatchesAsync().ConfigureAwait(false);
            }
        }

        private Task OnRefreshAsync()
        {
            Profiler.Start("ScoreItemViewModel.LoadMatches.PullDownToRefresh");
            return Task.Run(() => LoadDataAsync(() => UpdateMatchesAsync(true), false));
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

        [Time]
        private async Task LoadMatchesAsync(DateTime date, bool getLatestData = false)
        {
            var matches = IsLive
                ? await matchService
                    .GetLiveMatches(CurrentLanguage, getLatestData)
                    .ConfigureAwait(false)
                : await matchService
                    .GetMatchesByDate(date, CurrentLanguage, getLatestData)
                    .ConfigureAwait(false);

            var matchItemViewModels = matches
                    .Select(match => new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator))
                    .ToList();

            var groups = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match));

            Device
                .BeginInvokeOnMainThread(() =>
                {
                    MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(groups);
                });

            Profiler.Start("ScoresView.Render");
            Profiler.Stop("ScoreItemViewModel.LoadMatches.PullDownToRefresh");
            Debug.WriteLine($"Number of groups: {groups.Count()}");
            Debug.WriteLine($"Number of matches: {matchItemViewModels.Count}");
        }

        private async Task UpdateMatchesForTodayAndLiveMatchesAsync(bool getLatestData = false)
        {
            if (IsLive || SelectedDate == DateTime.Today || SelectedDate == DateTime.Today.AddDays(-1))
            {
                await Task
                    .Run(() => LoadDataAsync(() => UpdateMatchesAsync(getLatestData), false))
                    .ConfigureAwait(false);
            }
        }

        private async Task UpdateMatchesAsync(bool getLatestData = false)
        {
            try
            {
                var matches = IsLive
                        ? await matchService
                            .GetLiveMatches(CurrentLanguage, getLatestData)
                            .ConfigureAwait(false)
                        : await matchService
                            .GetMatchesByDate(SelectedDate, CurrentLanguage, getLatestData)
                            .ConfigureAwait(false);

                var matchViewModels = MatchItemsSource?.SelectMany(g => g).ToList();

                foreach (var match in matches)
                {
                    var matchViewModel
                        = matchViewModels?.FirstOrDefault(viewModel => viewModel.Match.Id == match.Id);

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
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex).ConfigureAwait(false);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => IsRefreshing = false);
            }
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

                var group = currentMatchViewModels
                        .OrderBy(m => m.Match.EventDate)
                        .GroupBy(item => new GroupMatchViewModel(item.Match))
                        .FirstOrDefault();

                Device
                    .BeginInvokeOnMainThread(() => MatchItemsSource[currentGroupIndex] = group);
            }
            else
            {
                currentMatchViewModels.Add(newMatchViewModel);

                var group = currentMatchViewModels
                       .OrderBy(m => m.Match.EventDate)
                       .GroupBy(item => new GroupMatchViewModel(item.Match))
                       .FirstOrDefault();

                Device
                    .BeginInvokeOnMainThread(() => MatchItemsSource.Add(group));
            }
        }
    }
}