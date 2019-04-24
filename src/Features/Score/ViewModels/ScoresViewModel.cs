using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scores.Tests")]

namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Models.Matches;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private readonly IMatchService MatchService;
        private DateRange selectedDateRange;

        public ScoresViewModel(
            INavigationService navigationService,
            IDepdendencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            SelectedDate = DateTime.Today;
            MatchService = DepdendencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.Value);

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(selectedDateRange, false, true));
        }

        public DateTime SelectedDate { get; internal set; }

        public bool IsLoading { get; set; }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchItemSource { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<IMatch> SelectMatchCommand { get; }

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
            }

            base.Dispose(disposing);
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            EventAggregator
               .GetEvent<DateBarItemSelectedEvent>()
               .Subscribe(OnDateBarItemSelected);

            if (MatchItemSource == null)
            {
                await LoadData(DateRange.FromYesterdayUntilNow());
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
                      matchData.GroupBy(match
                      => new { match.League.Name, match.EventDate.Day, match.EventDate.Month, match.EventDate.Year }));

            selectedDateRange = dateRange;
            IsLoading = false;
            IsRefreshing = false;
        }
    }
}