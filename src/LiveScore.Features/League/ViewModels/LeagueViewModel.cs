using System.Collections.ObjectModel;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeagueViewModel : ViewModelBase
    {
        public LeagueViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
                : base(navigationService, serviceLocator)
        {
            Title = "League";

            Leagues = new ObservableCollection<ILeague>();

            IsBusy = false;
            HasData = IsNotBusy;
        }

        public ObservableCollection<ILeague> Leagues { get; set; }
    }
}