namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class FAQPageViewModel : MenuViewModelBase
    {
        public FAQPageViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}