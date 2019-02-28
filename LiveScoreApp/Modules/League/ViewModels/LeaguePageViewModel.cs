namespace League.ViewModels
{
    using System.Collections.ObjectModel;
    using League.Models;
    using League.Services;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class LeaguePageViewModel : ViewModelBase
    {
        private League selectedTournament;

        public ObservableCollection<League> Leagues { get; set; }

        public DelegateCommand ItemTappedCommand { get; set; }

        public League SelectedTournament
        {
            get => selectedTournament;
            set => SetProperty(ref selectedTournament, value);
        }

        public LeaguePageViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            Title = "League";
            ItemTappedCommand = new DelegateCommand(ItemTapped);
            Leagues = new ObservableCollection<League>(leagueService.GetAll());
        }

        private async void ItemTapped()
        {
            var result = await NavigationService.NavigateAsync($"TournamentDetailPage?id={selectedTournament.Id}");

            if (!result.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Error loading tournament page", "Cancel");
            }
        }
    }
}