namespace LiveScore.Menu.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Prism.Navigation;

    public class DefaultSportViewModel : MenuViewModelBase
    {
        public DefaultSportViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}