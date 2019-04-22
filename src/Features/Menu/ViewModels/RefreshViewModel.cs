namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core;
    using Prism.Navigation;

    public class RefreshViewModel : MenuViewModelBase
    {
        public RefreshViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}