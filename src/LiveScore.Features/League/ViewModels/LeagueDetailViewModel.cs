namespace LiveScore.Features.League.ViewModels
{
    using Core;
    using Core.ViewModels;
    using Prism.Navigation;

    public class LeagueDetailViewModel : ViewModelBase
    {
        public LeagueDetailViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}