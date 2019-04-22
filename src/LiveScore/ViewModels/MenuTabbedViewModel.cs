namespace LiveScore.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class MenuTabbedViewModel : ViewModelBase
    {
        public MenuTabbedViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}