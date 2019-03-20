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
    using System.Diagnostics;

    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private League selectedLeague;

        public ObservableCollection<League> Leagues { get; set; }

        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateAsyncCommand LoadLeaguesCommand { get; set; }

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

        public League SelectedLeague
        {
            get => selectedLeague;
            set => SetProperty(ref selectedLeague, value);
        }

        public LeagueViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            Title = "League";
            this.leagueService = leagueService;
            ItemTappedCommand = new DelegateCommand(ItemTapped);
            LoadLeaguesCommand = new DelegateAsyncCommand(LoadLeaguesAsync);

            Leagues = new ObservableCollection<League>();
            IsLoading = true;
            HasData = !IsLoading;

            Debug.WriteLine("ctor LeagueViewModel");
        }
       
        public override void OnAppearing()
        {
            base.OnAppearing();

            if (Leagues.Count == 0)
            {
                Task.Run(LoadLeaguesCommand.ExecuteAsync).Wait();
            }
            else 
            {
                IsLoading = false;
                HasData = !IsLoading;
            }
        }

        private async void ItemTapped()
        {
            var result = await NavigationService.NavigateAsync($"LeagueDetailPage?id={selectedLeague.Id}");

            if (!result.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Error loading league page", "Cancel");
            }
        }

        private async Task LoadLeaguesAsync()
        {
            var leagues = await leagueService.GetAllAsync();

            foreach(var league in leagues)
            {
                Leagues.Add(league);
            }

            IsLoading = false;
            HasData = !IsLoading;
        }
    }
}