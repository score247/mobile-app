﻿namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Events;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Views;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private readonly DateTime today;
        private readonly IMatchService matchService;
        private DateRange selectedDateRange;

        public ScoresViewModel(
            INavigationService navigationService,
            IServiceLocator serviceLocator,
            IEventAggregator eventAggregator)
            : base(navigationService, serviceLocator, eventAggregator)
        {
            today = DateTime.Today;
            matchService = ServiceLocator.Create<IMatchService>(SettingsService.CurrentSport.GetDescription());

            SubscribeSportChangeEvent();

            SubscribeDateBarEvent();

            SetupCommands();
        }

        public bool IsLoading { get; private set; }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchItemSource { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public DelegateAsyncCommand<IMatch> SelectMatchCommand { get; private set; }

        public override async void OnResume()
        {
            if (today != DateTime.Today)
            {
                await NavigateToRoot();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            EventAggregator
                .GetEvent<DateBarItemSelectedEvent>()
                .Unsubscribe(OnDateBarItemSelected);
        }

        private void SubscribeDateBarEvent()
        {
            EventAggregator
                .GetEvent<DateBarItemSelectedEvent>()
                .Subscribe(OnDateBarItemSelected);
        }

        private void SetupCommands()
        {
            SelectMatchCommand = new DelegateAsyncCommand<IMatch>(NavigateToMatchDetailView);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(selectedDateRange, false, true));
        }

        private async Task NavigateToMatchDetailView(IMatch match)
            => await NavigationService.NavigateAsync(nameof(MatchDetailView), new NavigationParameters
            {
                { nameof(IMatch), match }
            });

        private async void OnDateBarItemSelected(DateRange dateRange)
        {
            await LoadData(dateRange);
        }

        private async Task LoadData(
            DateRange dateRange,
            bool showLoadingIndicator = true,
            bool forceFetchNewData = false,
            bool isRefreshing = false)
        {
            IsLoading = showLoadingIndicator;
            MatchItemSource?.Clear();

            var matchData = await matchService.GetMatches(
                    (int)SettingsService.CurrentSport,
                    SettingsService.CurrentLanguage,
                    dateRange ?? DateRange.FromYesterdayUntilNow(),
                    TimeZoneInfo.Local.BaseUtcOffset.ToString(),
                    forceFetchNewData);

            MatchItemSource = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matchData.GroupBy(match
                      => new { match.League.Name, match.EventDate.Day, match.EventDate.Month, match.EventDate.Year }));

            selectedDateRange = dateRange;
            IsLoading = false;
            IsRefreshing = isRefreshing;
        }
    }
}