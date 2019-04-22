namespace LiveScore.League.ViewModels
{
    using System.Collections.ObjectModel;
    using Core.Models.Leagues;
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class LeagueViewModel : ViewModelBase
    {
        public LeagueViewModel(
            INavigationService navigationService,
            IDepdendencyResolver serviceLocator)
                : base(navigationService, serviceLocator)
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