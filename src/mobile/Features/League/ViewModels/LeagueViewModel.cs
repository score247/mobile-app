namespace LiveScore.League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Constants;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Core.Models.Leagues;
    using LiveScore.League.Views;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private readonly IPageDialogService pageDialogService;
        private readonly List<League> leagueList;
        private bool isLoading;
        private bool hasData;
        private string filter;
        private ObservableCollection<League> leagues;


        public LeagueViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService,
            IPageDialogService pageDialogService)
                : base(navigationService, globalFactory, settingsService)
        {
            Title = "League";
            leagueService = GlobalFactoryProvider.SportServiceFactoryProvider.GetInstance((SportType)SettingsService.CurrentSportId).CreateLeagueService();
            this.pageDialogService = pageDialogService;

            ItemTappedCommand = new DelegateAsyncCommand<League>(ItemTapped);
            LoadLeaguesCommand = new DelegateAsyncCommand(GetLeagues);
            SearchCommand = new DelegateCommand(DelayedQueryKeyboardSearches);
            RefreshCommand = new DelegateAsyncCommand(Refresh);

            Leagues = new ObservableCollection<League>();
            leagueList = new List<League>();
            IsLoading = true;
            HasData = !IsLoading;
        }

        public DelegateAsyncCommand<League> ItemTappedCommand { get; set; }

        public DelegateAsyncCommand LoadLeaguesCommand { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; set; }

        public DelegateCommand SearchCommand { get; set; }

        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public bool HasData
        {
            get { return hasData; }
            set { SetProperty(ref hasData, value); }
        }

        public string Filter
        {
            get => filter;
            set => SetProperty(ref filter, value);
        }

        public ObservableCollection<League> Leagues
        {
            get => leagues;
            set => SetProperty(ref leagues, value);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (Leagues.Count == 0)
            {
                await LoadLeaguesCommand.ExecuteAsync();
            }
            else
            {
                IsLoading = false;
                HasData = !IsLoading;
            }
        }

        public override void Destroy()
        {
            // TODO clean all resources and requests
        }

        private async Task ItemTapped(League item)
        {
            var param = new NavigationParameters
            {
                { nameof(League), item }
            };

            var result = await NavigationService.NavigateAsync($"{nameof(LeagueDetailView)}", param);

            if (!result.Success)
            {
                await pageDialogService.DisplayAlertAsync("Alert", "Error loading league page", "Cancel");
            }
        }

        private async Task Refresh()
        {
            IsLoading = true;
            HasData = !IsLoading;

            leagueList.Clear();
            Leagues.Clear();

            await GetLeagues();
        }

        private async Task GetLeagues()
        {
            var leagueGroups = await leagueService.GetLeagues();

            Leagues = new ObservableCollection<League>(leagueGroups);

            leagueList.AddRange(leagues);

            IsLoading = false;
            HasData = !IsLoading;
        }

        private void DelayedQueryKeyboardSearches()
        {
            SearchLeagues(Filter);
        }

        private void SearchLeagues(string query)
        {
            var filterLeagues = leagueList;

            if (!string.IsNullOrWhiteSpace(query))
            {
                filterLeagues = leagueList
                        .Where(x => x.Name.ToLowerInvariant()
                        .Contains(query.ToLowerInvariant())).ToList();
            }

            Leagues = new ObservableCollection<League>(filterLeagues);
        }
    }
}