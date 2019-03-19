namespace LiveScoreApp.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MenuTabbedViewModel : ViewModelBase
    {
        public MenuTabbedViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}