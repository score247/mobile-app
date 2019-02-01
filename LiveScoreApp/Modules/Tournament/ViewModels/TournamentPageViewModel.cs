namespace Tournament.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Navigation;
    using System;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Tournament.Models;
    using Tournament.Services;

    public class TournamentPageViewModel : ViewModelBase
    {
        private Tournament selectedTournament;
        private ITournamentService tournamentService;

        public ObservableCollection<Tournament> Tournaments { get; set; }

        public DelegateCommand ItemTappedCommand { get; set; }

        public Tournament SelectedTournament
        {
            get => selectedTournament;
            set => SetProperty(ref selectedTournament, value);
        }

        public TournamentPageViewModel(INavigationService navigationService, ITournamentService tournamentService)
            : base(navigationService)
        {
            this.tournamentService = tournamentService;

            Title = "Tournament";
            ItemTappedCommand = new DelegateCommand(ItemTapped);

            Tournaments = new ObservableCollection<Tournament>(tournamentService.GetAll());
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