namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Constants;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Views;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private readonly DateRange currentDateRange;
        private IMatchService matchService;

        public ScoresViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
            InitializeCommands();
            currentDateRange = InitDateRange();
        }

        public bool IsLoading { get; set; }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchGroups { get; set; }

        public DelegateAsyncCommand MatchRefreshCommand { get; private set; }

        public DelegateAsyncCommand<IMatch> MatchSelectCommand { get; private set; }

        public DelegateAsyncCommand SelectDateBarHomeCommand { get; private set; }

        public DelegateAsyncCommand<DateBarItem> SelectDateBarItemCommand { get; private set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport || MatchGroups == null)
            {
                matchService = GlobalFactoryProvider
                   .ServiceFactoryProvider
                   .GetInstance((SportType)SettingsService.CurrentSportId)
                   .CreateMatchService();

                await LoadMatches(currentDateRange);
            }
        }

        private void InitializeCommands()
        {
            MatchSelectCommand = new DelegateAsyncCommand<IMatch>(OnSelectMatchCommand);
            MatchRefreshCommand = new DelegateAsyncCommand(OnRefreshMatchCommandAsync);
            SelectDateBarItemCommand = new DelegateAsyncCommand<DateBarItem>(OnSelectDateBarItemCommandAsync);
            SelectDateBarHomeCommand = new DelegateAsyncCommand(OnSelectDateBarHomeCommand);
        }

        private async Task OnSelectMatchCommand(IMatch match)
            => await NavigationService.NavigateAsync(nameof(MatchDetailView), new NavigationParameters
            {
                { nameof(IMatch), match }
            });

        private async Task OnRefreshMatchCommandAsync()
        {
            await LoadMatches(currentDateRange, showLoadingIndicator: false, forceFetchNewData: true).ConfigureAwait(false);
            IsRefreshing = false;
        }

        private async Task OnSelectDateBarHomeCommand()
        {
            if (!HomeIsSelected())
            {
                InitDateRange();
                await LoadMatches(currentDateRange);
            }
        }

        private async Task OnSelectDateBarItemCommandAsync(DateBarItem calendarDate)
        {
            if (CurrentDateIsNotSelectedDate(calendarDate.Date) || HomeIsSelected())
            {
                currentDateRange.FromDate = calendarDate.Date;
                currentDateRange.ToDate = calendarDate.Date;
                await LoadMatches(currentDateRange);
            }
        }

        private async Task LoadMatches(DateRange dateRange, bool showLoadingIndicator = true, bool forceFetchNewData = false)
        {
            IsLoading = showLoadingIndicator;

            if (dateRange == null)
            {
                dateRange = InitDateRange();
            }

            var matches = await matchService.GetMatches(
                    SettingsService.CurrentSportId,
                    SettingsService.CurrentLanguage,
                    dateRange.FromDate,
                    dateRange.ToDate,
                    forceFetchNewData);

            MatchGroups = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matches.GroupBy(m => new { m.League.Name, m.EventDate.Day, m.EventDate.Month, m.EventDate.Year }));

            IsLoading = false;
        }

        private static DateRange InitDateRange()
        {
            return new DateRange
            {
                FromDate = DateTime.Today.AddDays(-1),
                ToDate = DateTime.Today
            };
        }

        private bool CurrentDateIsNotSelectedDate(DateTime selectedDate)
            => currentDateRange.FromDate != selectedDate && currentDateRange.ToDate != selectedDate;

        private bool HomeIsSelected()
            => currentDateRange.FromDate != currentDateRange.ToDate;
    }
}