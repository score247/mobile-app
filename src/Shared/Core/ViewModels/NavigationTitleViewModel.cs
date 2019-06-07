namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core;
    using Prism.Navigation;

    public class NavigationTitleViewModel : ViewModelBase
    {
        public NavigationTitleViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            CurrentSportName = SettingsService.CurrentSportType.DisplayName;
        }

        public string CurrentSportName { get; }
    }
}