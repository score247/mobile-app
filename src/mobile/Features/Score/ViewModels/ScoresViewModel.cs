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
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Models;
    using LiveScore.Score.Views;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class ScoresViewModel : ViewModelBase
    {
        private const int NumberDisplayDays = 3;
        private IMatchService matchService;

        public ScoresViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
            InitializeCommands();
            SelectHome = true;
        }

        public bool IsLoadingMatches { get; set; }

        public bool IsRefreshingMatchList { get; set; }

        public bool SelectHome { get; set; }

        public CalendarDate SelectedCalendarDate { get; set; }

        public ObservableCollection<CalendarDate> CalendarItems { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> GroupMatches { get; set; }

        public DataTemplate MatchDataTemplate { get; set; }

        public DelegateAsyncCommand RefreshMatchListCommand { get; private set; }

        public DelegateAsyncCommand SelectDateCommand { get; private set; }

        public DelegateAsyncCommand SelectHomeCommand { get; private set; }

        public DelegateAsyncCommand<IMatch> SelectMatchCommand { get; private set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport || GroupMatches == null)
            {
                matchService = GlobalFactoryProvider
                   .ServiceFactoryProvider
                   .GetInstance((SportType)SettingsService.CurrentSportId)
                   .CreateMatchService();

                MatchDataTemplate = GlobalFactoryProvider
                    .TemplateFactoryProvider
                    .GetInstance((SportType)SettingsService.CurrentSportId)
                    .GetMatchTemplate();

                await LoadHomeData();
            }
        }

        private void InitializeCommands()
        {
            SelectMatchCommand = new DelegateAsyncCommand<IMatch>(OnSelectMatchCommand);
            RefreshMatchListCommand = new DelegateAsyncCommand(OnRefreshMatchListCommandAsync);
            SelectDateCommand = new DelegateAsyncCommand(OnSelectDateCommandAsync);
            SelectHomeCommand = new DelegateAsyncCommand(OnSelectHomeCommand);
        }

        private async Task LoadHomeData()
        {
            LoadCalendar(DateTime.MinValue);
            await LoadMatches(DateTime.Now.AddDays(-1), DateTime.Now);
        }

        private async Task OnSelectMatchCommand(IMatch match)
        {
            var navigationParams = new NavigationParameters
            {
                { nameof(IMatch), match }
            };

            await NavigationService.NavigateAsync(nameof(MatchDetailView), navigationParams);
        }

        private async Task OnRefreshMatchListCommandAsync()
        {
            var fromDate = SelectedCalendarDate == null ? DateTime.Today.AddDays(-1) : SelectedCalendarDate.Date;
            var toDate = SelectedCalendarDate == null ? DateTime.Today : SelectedCalendarDate.Date;
            await LoadMatches(fromDate, toDate, showLoadingIndicator: false, forceFetchNewData: true).ConfigureAwait(false);
            IsRefreshingMatchList = false;
        }

        private async Task OnSelectDateCommandAsync()
        {
            SelectHome = false;
            LoadCalendar(SelectedCalendarDate.Date);

            var fromDate = SelectedCalendarDate.Date;
            var toDate = SelectedCalendarDate.Date;
            await LoadMatches(fromDate, toDate).ConfigureAwait(false);
        }

        private async Task OnSelectHomeCommand()
        {
            SelectHome = true;
            await LoadHomeData();
        }

        private async Task LoadMatches(DateTime fromDate, DateTime toDate, bool showLoadingIndicator = true, bool forceFetchNewData = false)
        {
            IsLoadingMatches = showLoadingIndicator;

            var matches = await matchService.GetDailyMatches(fromDate, toDate, forceFetchNewData);
            GroupMatches = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matches.GroupBy(m => new { m.League.Name, m.EventDate.Day, m.EventDate.Month, m.EventDate.Year }));

            IsLoadingMatches = !IsLoadingMatches && showLoadingIndicator;
        }

        private void LoadCalendar(DateTime currentDate)
        {
            var calendar = new ObservableCollection<CalendarDate>();

            for (int i = -NumberDisplayDays; i <= NumberDisplayDays; i++)
            {
                var date = DateTime.Today.AddDays(i);

                calendar.Add(new CalendarDate
                {
                    Date = date,
                    IsSelected = currentDate.Day == date.Day
                        && currentDate.Month == date.Month
                        && currentDate.Year == date.Year
                });
            }

            CalendarItems = new ObservableCollection<CalendarDate>(calendar);
        }
    }
}