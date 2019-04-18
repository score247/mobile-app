namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class InfoAlertViewModel : MenuViewModelBase
    {
        public InfoAlertViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}