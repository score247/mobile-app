using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeaguesGroupViewModel : ViewModelBase
    {
        public LeaguesGroupViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, false)
        {
        }
    }
}