﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteTeamMatchesViewModel : MatchesViewModel
    {
        private readonly ITeamService teamService;
        private ITeamProfile currentTeam;

        public FavoriteTeamMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());
            LoadResultMatchesCommand = new DelegateCommand(OnLoadResultMatches);
            HeaderViewModel = this;
        }

        public DelegateCommand LoadResultMatchesCommand { get; }

        public bool ShowPreviousEvents { get; private set; }

        public bool ShowPreviousEventsButton { get; private set; }

        public string ShowPreviousEventsButtonText { get; private set; } = AppResources.ShowPreviousEvents;

        public IReadOnlyCollection<IGrouping<MatchGroupViewModel, MatchViewModel>> ResultMatchItemSource { get; private set; }

        public IReadOnlyCollection<IGrouping<MatchGroupViewModel, MatchViewModel>> ScheduleMatchItemSource { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters["Team"] is ITeamProfile team)
            {
                currentTeam = team;
            }

            Task.Delay(200).ContinueWith(async _ => await LoadDataAsync(LoadMatchesAsync));
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEvents();

            if (FirstLoad)
            {
                FirstLoad = false;
            }
            else
            {
                await Task.Run(() => LoadDataAsync(UpdateMatchesAsync, false));
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            SubscribeEvents();
            await Task.Run(() => LoadDataAsync(UpdateMatchesAsync, false));
        }

        protected override Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync()
            => teamService.GetTeamMatches(CurrentLanguage.DisplayName, currentTeam.Id);

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            var matchList = matches.ToList();

            var liveAndPreMatches = matchList
                .Where(match => match.EventStatus.IsLive || match.EventStatus.IsNotStarted)
                .ToList();

            base.InitializeMatchItems(liveAndPreMatches);

            BuildResultMatchItems(matchList, liveAndPreMatches);
            ShowHidePreviousEventButton();
        }

        protected override void UpdateMatchItems(IEnumerable<IMatch> matches)
        {
            var matchList = matches?.ToList();

            try
            {
                if ((MatchItemsSource == null || MatchItemsSource.Count == 0) && matchList?.Any() == true)
                {
                    InitializeMatchItems(matchList);
                    return;
                }

                var loadedYesterdayOrTodayMatches = matchList
                        .Where(match => MatchItemsSource.Any(item =>
                            item.Any(matchItem => matchItem?.Match != null && matchItem.Match.Id == match.Id &&
                              matchItem.Match.EventDate.IsFromYesterdayUntilNow())));

                Device.BeginInvokeOnMainThread(() =>
                    MatchItemsSource.UpdateMatchItems(
                        loadedYesterdayOrTodayMatches,
                        matchStatusBuilder,
                        matchMinuteBuilder,
                        EventAggregator,
                        buildFlagUrlFunc,
                        NavigationService,
                        CurrentSportId,
                        favoriteService));
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
            }
        }

        private void BuildResultMatchItems(IEnumerable<IMatch> matchList, IEnumerable<IMatch> liveAndPreMatches)
        {
            var closedMatchViewModels = matchList
                .Except(liveAndPreMatches)
                .Select(match
                    => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator, favoriteService));

            var closedMatchItems = closedMatchViewModels
                .GroupBy(item
                    => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId,
                        EnableTapLeague));

            ResultMatchItemSource = closedMatchItems.ToList();
        }

        private void ShowHidePreviousEventButton()
        {
            if (ResultMatchItemSource.Count > 0)
            {
                ShowPreviousEvents = true;
                ShowPreviousEventsButton = true;
                ShowPreviousEventsButtonText = AppResources.ShowPreviousEvents;
                HeaderViewModel = this;
            }
            else
            {
                HeaderViewModel = null;
            }
        }

        private void OnLoadResultMatches()
        {
            if (ResultMatchItemSource?.Any() != true)
            {
                return;
            }

            if (ShowPreviousEvents)
            {
                ScheduleMatchItemSource ??= new List<IGrouping<MatchGroupViewModel, MatchViewModel>>(MatchItemsSource);

                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentItem = MatchItemsSource.First();

                    foreach (var matchGroup in ResultMatchItemSource)
                    {
                        MatchItemsSource.Insert(0, matchGroup);
                    }

                    ScrollToCommand?.Execute(currentItem);
                    ShowPreviousEventsButtonText = AppResources.HidePreviousEvents;
                    ShowPreviousEvents = false;
                });
            }
            else
            {
                MatchItemsSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(ScheduleMatchItemSource);
                ShowPreviousEventsButtonText = AppResources.ShowPreviousEvents;
                ShowPreviousEvents = true;
            }
        }
    }
}