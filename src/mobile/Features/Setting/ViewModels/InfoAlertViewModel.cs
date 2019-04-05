namespace LiveScore.Menu.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Prism.Navigation;

    public class InfoAlertViewModel : MenuViewModelBase
    {
        public InfoAlertViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}