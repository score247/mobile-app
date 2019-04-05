namespace LiveScore.Score.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Navigation;

    public class MatchTrackerViewModel : ViewModelBase
    {
        public MatchTrackerViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}