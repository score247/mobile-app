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
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Controls.QuickAccessCalendar.Models;
    using LiveScore.Score.Views;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class ScoresViewModel : ViewModelBase
    {
        private const int NumberDisplayDays = 3;
        private DateTime currentDate = DateTime.MinValue;
        private IMatchService matchService;

        public ScoresViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
            InitializeCommands();

            QuickAccessCalendarDateRange = new QuickAccessCalendarDateRange
            {
                FromDate = DateTime.Today.AddDays(-NumberDisplayDays),
                ToDate = DateTime.Today.AddDays(NumberDisplayDays)
            };
        }

        public bool IsLoading { get; set; }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchGroups { get; set; }

        public DelegateAsyncCommand MatchRefreshCommand { get; private set; }

        public DelegateAsyncCommand<IMatch> MatchSelectCommand { get; private set; }

        public QuickAccessCalendarDateRange QuickAccessCalendarDateRange { get; set; }

        public DelegateAsyncCommand QuickAccessCalendarSelectHomeCommand { get; private set; }

        public DelegateAsyncCommand<QuickAccessCalendarDate> QuickAccessCalendarSelectDateCommand { get; private set; }

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

                await LoadMatches();
            }
        }

        private void InitializeCommands()
        {
            MatchSelectCommand = new DelegateAsyncCommand<IMatch>(OnSelectMatchCommand);
            MatchRefreshCommand = new DelegateAsyncCommand(OnRefreshMatchCommandAsync);
            QuickAccessCalendarSelectDateCommand = new DelegateAsyncCommand<QuickAccessCalendarDate>(OnSelectDateCommandAsync);
            QuickAccessCalendarSelectHomeCommand = new DelegateAsyncCommand(OnSelectHomeCommand);
        }

        private async Task OnSelectMatchCommand(IMatch match)
            => await NavigationService.NavigateAsync(nameof(MatchDetailView), new NavigationParameters
            {
                { nameof(IMatch), match }
            });

        private async Task OnRefreshMatchCommandAsync()
        {
            await LoadMatches(currentDate, showLoadingIndicator: false, forceFetchNewData: true).ConfigureAwait(false);
            IsRefreshing = false;
        }

        private async Task OnSelectHomeCommand() => await LoadMatches();

        private async Task OnSelectDateCommandAsync(QuickAccessCalendarDate calendarDate)
        {
            if (currentDate != calendarDate.Date)
            {
                currentDate = calendarDate.Date;
                await LoadMatches(currentDate);
            }
        }

        private async Task LoadMatches(DateTime? date = null, bool showLoadingIndicator = true, bool forceFetchNewData = false)
        {
            IsLoading = showLoadingIndicator;


            var fromDate = date ?? DateTime.Today.AddDays(-1);
            var toDate = date ?? DateTime.Today;

            var matches = await matchService.GetMatches(SettingsService.CurrentSportId, SettingsService.CurrentLanguage, fromDate, toDate, forceFetchNewData);
            var GroupMatches = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matches.GroupBy(m => new { m.League.Name, m.EventDate.Day, m.EventDate.Month, m.EventDate.Year }));

            IsLoading = false;
        }
    }
}