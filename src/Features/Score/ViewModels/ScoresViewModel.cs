﻿using System.Runtime.CompilerServices;

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
    using LiveScore.Common.Configuration;
    using LiveScore.Core;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private const int HubKeepAliveInterval = 30;
        private readonly IMatchService MatchService;
        private readonly HubConnection matchHubConnection;
        private DateRange selectedDateRange;
        private CancellationTokenSource cancellationTokenSource;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = DateTime.Today;
            MatchService = DepdendencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.Value);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(selectedDateRange, false, true));
            matchHubConnection = new HubConnectionBuilder().WithUrl($"{Configuration.LocalHubEndPoint}/MatchHub").Build();
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsLoading { get; set; }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, MatchItemSourceViewModel>> MatchItemSource { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand ClickSearchCommand => new DelegateAsyncCommand(OnClickSearchCommandExecuted);

        private async Task OnClickSearchCommandExecuted()
        {
            await NavigationService.NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true);
        }

        public override async void OnResume()
        {
            await Initialize();

            if (SelectedDate != DateTime.Today)
            {
                await NavigateToHome();
            }
        }

        public override async void OnAppearing()
        {
            await Initialize();
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (MatchItemSource == null)
            {
                await LoadData(DateRange.FromYesterdayUntilNow());
            }
        }

        private async Task Initialize()
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

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }

        private async void OnDateBarItemSelected(DateRange dateRange) => await LoadData(dateRange);

        private async Task LoadData(
            DateRange dateRange,
            bool showLoadingIndicator = true,
            bool forceFetchNewData = false)
        {
            IsLoading = showLoadingIndicator;

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
            IsLoading = false;
            IsRefreshing = false;
        }

        private ObservableCollection<IGrouping<dynamic, MatchItemSourceViewModel>> BuildMatchItemSource(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(
                    match => new MatchItemSourceViewModel(match, NavigationService, DepdendencyResolver, EventAggregator, matchHubConnection));

            return new ObservableCollection<IGrouping<dynamic, MatchItemSourceViewModel>>(matchItemViewModels.GroupBy(item
                => new { item.Match.League.Name, item.Match.EventDate.Day, item.Match.EventDate.Month, item.Match.EventDate.Year }));
        }

        private void OnMatchesChanged(string sportId, Dictionary<string, MatchPayload> matchPayloads)
        {
            if (sportId != SettingsService.CurrentSportType.Value)
            {
                return;
            }

            var matchItem = MatchItemSource
                .SelectMany(group => group)
                .FirstOrDefault(m => matchPayloads.ContainsKey(m.Match.Id));

            if (matchItem?.Match != null)
            {
                var matchPayload = matchPayloads[matchItem.Match.Id];
                matchItem.Match.MatchResult = matchPayload.MatchResult;
                matchItem.Match.TimeLines = matchPayload.Timelines;

                matchItem.ChangeMatchData();
            }
        }
    }
}