namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class DefaultLanguageViewModel : MenuViewModelBase
    {
        public DefaultLanguageViewModel(INavigationService navigationService, IServiceLocator serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}