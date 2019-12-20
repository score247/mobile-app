using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeagueGroupStagesViewModel : ViewModelBase
    {
        public LeagueGroupStagesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, false)
        {
        }
    }
}