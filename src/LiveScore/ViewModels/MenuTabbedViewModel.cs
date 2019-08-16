namespace LiveScore.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class MenuTabbedViewModel : ViewModelBase
    {
        public MenuTabbedViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}