using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scores.Tests")]

namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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
        private readonly IMatchService MatchService;
        private readonly HubConnection matchHubConnection;
        private DateRange selectedDateRange;

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

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchItemSource { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public override async void OnResume()
        {
            if (SelectedDate != DateTime.Today)
            {
                await NavigateToHome();
            }
        }

        public override void OnDisappearing() => Dispose(true);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventAggregator
                    .GetEvent<DateBarItemSelectedEvent>()
                    .Unsubscribe(OnDateBarItemSelected);

                matchHubConnection.StopAsync().Wait();
            }

            base.Dispose(disposing);
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            try
            {
                EventAggregator
                   .GetEvent<DateBarItemSelectedEvent>()
                   .Subscribe(OnDateBarItemSelected);

                await matchHubConnection.StartAsync();

                MatchService.SubscribeMatch(matchHubConnection, UpdateMatch);

                if (MatchItemSource == null)
                {
                    await LoadData(DateRange.FromYesterdayUntilNow());
                }
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }



#pragma warning disable S3168 // "async" methods should not return "void"

        private async void OnDateBarItemSelected(DateRange dateRange) => await LoadData(dateRange);

#pragma warning restore S3168 // "async" methods should not return "void"

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

            var matchData = await MatchService.GetMatches(
                    SettingsService.UserSettings,
                    dateRange ?? DateRange.FromYesterdayUntilNow(),
                    forceFetchNewData);

            MatchItemSource = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matchData.GroupBy(match => new { match.League.Name, match.EventDate.Day, match.EventDate.Month, match.EventDate.Year }));

            selectedDateRange = dateRange;
            IsLoading = false;
            IsRefreshing = false;
        }

        private void UpdateMatch(string sportId, IDictionary<string, MatchPayload> payloads)
        {
            if (sportId == SettingsService.CurrentSportType.Value)
            {
                foreach (var league in MatchItemSource)
                {
                    foreach (var match in league)
                    {
                        if (payloads.ContainsKey(match.Id))
                        {
                            var payload = payloads[match.Id];

                            match.MatchResult = payload.MatchResult;
                            match.TimeLines = payload.Timelines;
                        }
                    }
                }

                MatchItemSource = new ObservableCollection<IGrouping<dynamic, IMatch>>(MatchItemSource);
            }
        }
    }
}