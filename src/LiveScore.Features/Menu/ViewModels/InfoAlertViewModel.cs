using LiveScore.Core;
using Prism.Navigation;

namespace LiveScore.Features.Menu.ViewModels
{
    public class InfoAlertViewModel : MenuViewModelBase
    {
        public InfoAlertViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}