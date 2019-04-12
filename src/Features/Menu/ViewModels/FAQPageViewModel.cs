namespace LiveScore.Menu.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Prism.Navigation;

    public class FAQPageViewModel : MenuViewModelBase
    {
        public FAQPageViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}