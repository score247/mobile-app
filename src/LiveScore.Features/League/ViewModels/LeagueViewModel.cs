namespace LiveScore.Features.League.ViewModels
{
    using System.Collections.ObjectModel;
    using Core.Models.Leagues;
    using Core.ViewModels;
    using Core;
    using Prism.Navigation;

    public class LeagueViewModel : ViewModelBase
    {
        public LeagueViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
                : base(navigationService, serviceLocator)
        {
            Title = "League";

            Leagues = new ObservableCollection<ILeague>();

            IsLoading = false;
            HasData = !IsLoading;
        }

        public bool HasData { get; set; }

        public ObservableCollection<ILeague> Leagues { get; set; }
    }
}