namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core;
    using Prism.Navigation;

    public class DefaultSportViewModel : MenuViewModelBase
    {
        public DefaultSportViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}