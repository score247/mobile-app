namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core;
    using Prism.Navigation;

    public class DefaultLanguageViewModel : MenuViewModelBase
    {
        public DefaultLanguageViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}