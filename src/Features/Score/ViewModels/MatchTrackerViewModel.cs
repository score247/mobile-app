namespace LiveScore.Score.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class MatchTrackerViewModel : ViewModelBase
    {
        public MatchTrackerViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}