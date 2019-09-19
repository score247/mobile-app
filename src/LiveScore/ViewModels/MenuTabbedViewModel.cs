using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.ViewModels
{
    public class MenuTabbedViewModel : ViewModelBase
    {
        public MenuTabbedViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}