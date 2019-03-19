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

    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private League selectedLeague;

        public ObservableCollection<League> Leagues { get; set; }

        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand LoadLeaguesCommand { get; set; }

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
            LoadLeaguesCommand = new DelegateCommand(async () => await LoadLeagues());

            LoadLeaguesCommand.Execute();
        }

        private async void ItemTapped()
        {
            var result = await NavigationService.NavigateAsync($"LeagueDetailPage?id={selectedLeague.Id}");

            if (!result.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Error loading tournament page", "Cancel");
            }
        }

        private async Task LoadLeagues()
        {
            //IsLoadingMatches = showLoadingIndicator;

            var leagues = await leagueService.GetAllAsync("eu", "soccer", "en");

            Leagues = new ObservableCollection<League>(leagues);           

        }
    }
}