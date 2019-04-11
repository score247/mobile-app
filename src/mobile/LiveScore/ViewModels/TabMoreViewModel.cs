namespace LiveScore.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class TabMoreViewModel : ViewModelBase
    {
        public TabMoreViewModel(INavigationService navigationService, IGlobalFactoryProvider globalFactory, ISettingsService settingsService) 
            : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}
