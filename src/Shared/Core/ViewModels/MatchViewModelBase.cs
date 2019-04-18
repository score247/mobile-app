namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using Prism.Navigation;

    public class MatchViewModelBase : ViewModelBase
    {
        public MatchViewModelBase(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService,
            IServiceLocator serviceLocator) : base(navigationService, globalFactory, settingsService, serviceLocator)
        {
        }

        public string MatchId { get; set; }
    }
}