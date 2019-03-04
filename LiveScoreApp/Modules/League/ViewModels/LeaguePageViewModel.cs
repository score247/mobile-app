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
        private League selectedLeague;

        public ObservableCollection<League> Leagues { get; set; }

        public DelegateCommand ItemTappedCommand { get; set; }

        public League SelectedLeague
        {
            get => selectedLeague;
            set => SetProperty(ref selectedLeague, value);
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
            var result = await NavigationService.NavigateAsync($"LeagueDetailPage?id={selectedLeague.Id}");

            if (!result.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Error loading tournament page", "Cancel");
            }
        }
    }
}