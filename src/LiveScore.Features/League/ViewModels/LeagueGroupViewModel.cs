using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeagueGroupViewModel : ViewModelBase
    {
        public LeagueGroupViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, false)
        {
        }
    }
}