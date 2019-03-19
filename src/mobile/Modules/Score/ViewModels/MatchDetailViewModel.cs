namespace Score.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MatchDetailViewModel : ViewModelBase
    {
        public MatchDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}