using LiveScore.Core;
using Prism.Navigation;

namespace LiveScore.Features.Menu.ViewModels
{
    public class RefreshViewModel : MenuViewModelBase
    {
        public RefreshViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}