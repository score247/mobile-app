namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class DefaultSportViewModel : MenuViewModelBase
    {
        public DefaultSportViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}