namespace LiveScore.Features.Menu.ViewModels
{
    using Core;
    using Prism.Navigation;

    public class FaqPageViewModel : MenuViewModelBase
    {
        public FaqPageViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}