using LiveScore.Core;
using Prism.Navigation;

namespace LiveScore.Features.Menu.ViewModels
{
    public class DefaultLanguageViewModel : MenuViewModelBase
    {
        public DefaultLanguageViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}