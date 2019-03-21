namespace League.ViewModels
{
    using System.Collections.ObjectModel;
    using Common.ViewModels;
    using Common.Models.MatchInfo;
    using League.Services;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;
    using System.Threading.Tasks;
    using Common.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using League.Views;

    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private string filter;
        private List<Category> leagueList;

        public ObservableCollection<Category> Leagues { get; set; }

        public DelegateAsyncCommand<Category> ItemTappedCommand { get; set; }
        public DelegateAsyncCommand LoadLeaguesCommand { get; set; }

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

        public string Filter
        {
            get => filter;
            set => SetProperty(ref filter, value);
        }

        public LeagueViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            Title = "League";
            this.leagueService = leagueService;

            ItemTappedCommand = new DelegateAsyncCommand<Category>(ItemTapped);
            LoadLeaguesCommand = new DelegateAsyncCommand(LoadLeaguesAsync);
            SearchCommand = new DelegateCommand(SearchLeagues);           

            Leagues = new ObservableCollection<Category>();
            leagueList = new List<Category>();
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

        private async Task ItemTapped(Category Item)
        {
            var result = await NavigationService.NavigateAsync($"{nameof(LeagueDetailView)}?id={Item.Id}");

            if (!result.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Error loading league page", "Cancel");
            }
        }

        private async Task LoadLeaguesAsync()
        {
            var leagues = await leagueService.GetCategories();

            foreach(var league in leagues)
            {
                Leagues.Add(league);
            }

            leagueList.AddRange(leagues);

            IsLoading = false;
            HasData = !IsLoading;
        }

        private void SearchLeagues() 
        {
            var filterLeagues = leagueList;

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                filterLeagues = leagueList.Where(x => x.Name.ToLower().Contains(Filter.ToLower())).ToList();
            }

            Leagues.Clear();

            foreach (var league in filterLeagues)
            {
                Leagues.Add(league);
            }
        }
       
    }
}