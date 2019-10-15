using LiveScore.Core;
using Prism.Navigation;

namespace LiveScore.Features.Menu.ViewModels
{
    public class DefaultSportViewModel : MenuViewModelBase
    {
        public DefaultSportViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}