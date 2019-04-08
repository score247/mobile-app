namespace LiveScore.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Navigation;

    public class MenuTabbedViewModel : ViewModelBase
    {
        public MenuTabbedViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}