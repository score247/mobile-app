namespace LiveScore.Features.Menu.ViewModels
{
    using LiveScore.Core;
    using Prism.Navigation;

    public class FaqPageViewModel : MenuViewModelBase
    {
        public FaqPageViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}