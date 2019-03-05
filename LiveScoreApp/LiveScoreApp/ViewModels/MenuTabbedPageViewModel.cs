namespace LiveScoreApp.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MenuTabbedPageViewModel : ViewModelBase
    {
        public MenuTabbedPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}