namespace LiveScore.Features.Menu.ViewModels
{
    using Core;
    using Prism.Navigation;

    public class DefaultSportViewModel : MenuViewModelBase
    {
        public DefaultSportViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}