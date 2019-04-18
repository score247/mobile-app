namespace LiveScore.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class MenuTabbedViewModel : ViewModelBase
    {
        public MenuTabbedViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}