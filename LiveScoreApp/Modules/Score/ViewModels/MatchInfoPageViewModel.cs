namespace Score.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MatchInfoPageViewModel : ViewModelBase
    {
        public MatchInfoPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}