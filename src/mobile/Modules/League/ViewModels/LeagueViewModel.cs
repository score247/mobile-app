namespace League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.ViewModels;
    using League.Models;
    using League.Services;
    using League.Views;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private readonly IPageDialogService pageDialogService;

        private readonly List<LeagueItem> leagueList;

        public DelegateAsyncCommand<LeagueItem> ItemTappedCommand { get; set; }
        public DelegateAsyncCommand LoadLeaguesCommand { get; set; }
        public DelegateAsyncCommand RefreshCommand { get; set; }

        public DelegateCommand SearchCommand { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private bool hasData;
        public bool HasData
        {
            get { return hasData; }
            set { SetProperty(ref hasData, value); }
        }

        private string filter;
        public string Filter
        {
            get => filter;
            set => SetProperty(ref filter, value);
        }

        private ObservableCollection<LeagueItem> leagues;
        public ObservableCollection<LeagueItem> Leagues
        {
            get => leagues;
            set => SetProperty(ref leagues, value);
        }

        public LeagueViewModel(INavigationService navigationService, ILeagueService leagueService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "League";
            this.leagueService = leagueService;
            this.pageDialogService = pageDialogService;

            ItemTappedCommand = new DelegateAsyncCommand<LeagueItem>(ItemTapped);
            LoadLeaguesCommand = new DelegateAsyncCommand(GetLeagueCategories);
            SearchCommand = new DelegateCommand(DelayedQueryKeyboardSearches);
            RefreshCommand = new DelegateAsyncCommand(Refresh);

            Leagues = new ObservableCollection<LeagueItem>();
            leagueList = new List<LeagueItem>();
            IsLoading = true;
            HasData = !IsLoading;
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

        private async Task ItemTapped(LeagueItem Item)
        {
            var result = await NavigationService.NavigateAsync($"{nameof(LeagueDetailView)}?id={Item.Id}");

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

            await GetLeagueCategories();
        }

        private async Task GetLeagueCategories()
        {
            var leagueGroups = await leagueService.GetLeaguesAsync();

            Leagues = new ObservableCollection<LeagueItem>(leagueGroups);

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

            Leagues = new ObservableCollection<LeagueItem>(filterLeagues);
        }

        //TODO clean all resources and requests
        public override void Destroy()
        {
            base.Destroy();
        }
    }
}