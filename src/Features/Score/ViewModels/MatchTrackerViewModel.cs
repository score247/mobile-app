namespace LiveScore.Score.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class MatchTrackerViewModel : ViewModelBase
    {
        public MatchTrackerViewModel(INavigationService navigationService, IServiceLocator serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}