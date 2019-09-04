namespace LiveScore.Features.Menu.ViewModels
{
    using Core;
    using Prism.Navigation;

    public class DefaultLanguageViewModel : MenuViewModelBase
    {
        public DefaultLanguageViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}