namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class RefreshViewModel : MenuViewModelBase
    {
        public RefreshViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}