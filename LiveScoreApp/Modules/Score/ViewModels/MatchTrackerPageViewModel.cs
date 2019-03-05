namespace Score.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MatchTrackerPageViewModel : ViewModelBase
    {
        public MatchTrackerPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}