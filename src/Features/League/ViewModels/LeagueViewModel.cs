namespace LiveScore.League.ViewModels
{
    using System.Collections.ObjectModel;
    using Core.Factories;
    using Core.Models.Leagues;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Navigation;

    public class LeagueViewModel : ViewModelBase
    {
        public LeagueViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
        {
            Title = "League";

            Leagues = new ObservableCollection<ILeague>();

            IsLoading = false;
            HasData = !IsLoading;
        }

        public bool IsLoading { get; set; }

        public bool HasData { get; set; }

        public ObservableCollection<ILeague> Leagues { get; set; }

    }
}