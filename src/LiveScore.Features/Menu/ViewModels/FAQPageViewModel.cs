using LiveScore.Core;
using Prism.Navigation;

namespace LiveScore.Features.Menu.ViewModels
{
    public class FaqPageViewModel : MenuViewModelBase
    {
        public FaqPageViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}