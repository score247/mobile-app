namespace Score.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MatchTrackerViewModel : ViewModelBase
    {
        public MatchTrackerViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}