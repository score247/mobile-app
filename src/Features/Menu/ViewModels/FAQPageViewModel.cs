namespace LiveScore.Menu.ViewModels
{
    using LiveScore.Core;
    using Prism.Navigation;

    public class FaqPageViewModel : MenuViewModelBase
    {
        public FaqPageViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}